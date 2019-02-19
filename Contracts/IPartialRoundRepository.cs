using System;
using Entities.Models;

namespace Contracts
{
    public interface IPartialRoundRepository
    {
        void CreatePartialRound(PartialRound partialRound);
        PartialRound GetPartialRound(Guid id);
    }
}
