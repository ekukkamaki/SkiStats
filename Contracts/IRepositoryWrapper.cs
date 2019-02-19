using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public interface IRepositoryWrapper
    {
        IRoundRepository Round { get; }
        IPersonRepository Person { get; }
        ILocationRepository Location { get; }
    }
}
