using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json;
using Test.Models;
using static System.Net.Mime.MediaTypeNames;

namespace Test.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class DogController : ControllerBase
    {
        private DataContext _dataContext;
        private readonly IConfiguration _configuration;
        private static bool _isValidJson(string strInput)
        {
            if (string.IsNullOrWhiteSpace(strInput)) { return false; }
            strInput = strInput.Trim();
            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || 
                (strInput.StartsWith("[") && strInput.EndsWith("]")))
            {
                try
                {
                    var obj = JToken.Parse(strInput);
                    return true;
                }
                catch (JsonReaderException jex)
                {
                    Console.WriteLine(jex.Message);
                    return false;
                }
                catch (Exception ex) 
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public DogController(DataContext data,IConfiguration config)
        {
            _dataContext = data;
            _configuration= config;
        }

        [HttpGet]
        [Route("Ping")]
        public ActionResult Ping()
        {
            return Ok(_configuration.GetValue<string>("Versioning:ApiVersion"));
        }
        

        [HttpPost]
        [Route("AddDog")]
        public async Task<ActionResult<Dog>> AddDog(string json)
        {
            if (_isValidJson(json))
            {
                if (json is not null)
                {
                    var newDog = JsonConvert.DeserializeObject<Dog>(json);
                    if (newDog is not null)
                    {
                        if (newDog.Tail_Length < 13 && newDog.Tail_Length.GetType() == typeof(int) && newDog.Tail_Length > 0)
                        {
                            var a = await _dataContext.Dogs.FirstOrDefaultAsync(x=>x.Name==newDog.Name);
                            if (a is null)
                            {
                                await _dataContext.AddAsync(newDog);
                                await _dataContext.SaveChangesAsync();
                                return Ok(a);
                            }
                            else
                            {
                                return BadRequest(newDog.Name + " Alredy exsist");
                            }
                        }
                    }
                    else
                        return BadRequest("Wrong value");

                }
            }
            return BadRequest("Wrong value");
        }
       
        
        [HttpGet]
        [Route("GetDogs")]
        public async Task<ActionResult<IEnumerable<Dog>>> GetDogs(int page=0,string orderBy= "Name", int pageSize=2)
        {
            var str = Request.Host.ToString();
            if (page.GetType() == typeof(int) && page >= 0)
            {
                var dogResponse = new DogResponse
                {
                    NextPage ="https://"+ Request.Host.ToString() + $"/api/Dog/GetDogs?page={page+1}&orderBy={orderBy}&pageSize={pageSize}",
                    PreviousPage = "https://"+Request.Host.ToString() + $"/api/Dog/GetDogs?page=" + (page > 0 ? page-1 : page) + $"&orderBy={orderBy}&pageSize={pageSize}",
                    SelectedPage = page
                };
                var dogs = await _dataContext.Dogs.Skip(page * pageSize).Take(pageSize).ToListAsync();
                if (page > PaginationManager.Maxpages(dogs))
                {
                    return BadRequest("Such page does not exsist");
                }
                switch (orderBy)
                {
                    case "Name":
                        dogResponse.DogList = dogs.OrderBy(x => x.Name).ToList();
                        break;
                    case "Weight":
                        dogResponse.DogList = dogs.OrderBy(x => x.Name).ToList();
                        break;

                    case "Tail_Length":
                        dogResponse.DogList = dogs.OrderBy(x => x.Name).ToList();
                        break;

                    case "Color":
                        dogResponse.DogList = dogs.OrderBy(x => x.Name).ToList();
                        break;

                }
                return Ok(dogResponse);
            }
            else
            {
                return BadRequest("Such page does not exsist");
            }
        }
    }
}
