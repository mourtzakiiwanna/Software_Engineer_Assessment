using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class CountryDbContext : DbContext
    {
        public DbSet<CountryDbModel> Countries { get; set; }

        public CountryDbContext(DbContextOptions<CountryDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
