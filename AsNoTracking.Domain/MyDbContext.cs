using Microsoft.EntityFrameworkCore;

namespace AsNoTracking.Domain
{
    public class MyDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        // Other DbSets and configurations

        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
        }
    }
}
