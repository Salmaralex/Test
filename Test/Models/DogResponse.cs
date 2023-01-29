namespace Test.Models
{
    public class DogResponse
    {
        public List<Dog> DogList { get;set; } 
        public string PreviousPage { get;set; }
        public string NextPage { get;set; }
        public int SelectedPage { get; set; }
    }
}
