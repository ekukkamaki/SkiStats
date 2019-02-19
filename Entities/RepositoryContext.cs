using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Entities
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext(DbContextOptions<RepositoryContext> options) : base(options)
        {
        }

        public DbSet<Person> Persons { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Round> Rounds { get; set; }
        public DbSet<PartialRound> PartialRounds { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Person>().HasKey(r => new {r.Id});
            modelBuilder.Entity<Round>().HasOne(r => r.Person).WithMany(e => e.Rounds);
            modelBuilder.Entity<Round>().HasOne(r => r.Location);

            modelBuilder.Entity<Location>().HasKey(r => new { r.Id });

            modelBuilder.Entity<Round>().HasMany(r => r.PartialRounds).WithOne(o => o.Round);
        }
    }
}
