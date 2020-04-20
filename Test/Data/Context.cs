using Microsoft.EntityFrameworkCore;
using Test.Models;

namespace Test.Data
{
    public class Context:DbContext
    {
        public Context(DbContextOptions<Context> options):base(options)
        {
            
        }
        public DbSet<UserDetail> User { get; set; }
    }
}