using System;
using System.Collections.Generic;
using System.Text;
using Entities.Models;

namespace Contracts
{
    public interface ILocationRepository
    {
        void AddLocation(Location location);
        Location GetById(Guid id);
    }
}
