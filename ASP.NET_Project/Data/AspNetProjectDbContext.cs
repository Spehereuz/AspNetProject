using ASP.NET_Project.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET_Project.Data
{
    public class AspNetProjectDbContext : DbContext
    {
        public AspNetProjectDbContext(DbContextOptions<AspNetProjectDbContext> options) : base(options) { }

        public DbSet<CategoryEntity> Categories { get; set; }
    }
}
