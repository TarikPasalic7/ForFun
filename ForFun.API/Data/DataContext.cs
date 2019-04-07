using Microsoft.EntityFrameworkCore;
using ForFun.API.Models;
namespace ForFun.API.Data
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
     
     public DbSet<Values> Values { get; set; }
    
   
    }
}

