using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Contracts;
using Entities;
using Entities.Models;

namespace Repository
{
    public class PartialRoundRepository : RepositoryBase<PartialRound>, IPartialRoundRepository
    {
        public PartialRoundRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public void CreatePartialRound(PartialRound partialRound)
        {
            partialRound.Id = Guid.NewGuid();
            Create(partialRound);
            Save();
        }

        public PartialRound GetPartialRound(Guid id)
        {
            return FindByCondition(t => t.Id == id).Single();
        }

        public IEnumerable<PartialRound> GetPartialRoundsByRoundId(Guid roundId)
        {
            return FindByCondition(t => t.Round.Id == roundId).ToList();

        }
    }
}
