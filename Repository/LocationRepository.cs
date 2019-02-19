using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Contracts;
using Entities;
using Entities.Models;

namespace Repository
{
    public class LocationRepository : RepositoryBase<Location>, ILocationRepository
    {
        public LocationRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public void AddLocation(Location location)
        {
            location.Id = Guid.NewGuid();
            Create(location);
            Save();
        }

        public Location GetById(Guid id)
        {
            return FindByCondition(f => f.Id == id).Single();
        }
    }
}
