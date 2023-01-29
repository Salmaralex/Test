using Microsoft.EntityFrameworkCore;

namespace Test.Models
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        
        public DbSet<Dog> Dogs { get; set; }
        
    }
}
