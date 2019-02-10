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
    }
}
