using System;
using System.Collections.Generic;
using System.Text;
using Entities.Models;

namespace Contracts
{
    public interface IRoundRepository
    {
        IEnumerable<Round> GetAllRounds();
        void CreateRound(Round round);
        Round GetByRoundId(Guid id);
    }
}
