using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Contracts;
using Entities;
using Entities.Models;

namespace Repository
{
    public class RoundRepository : RepositoryBase<Round>, IRoundRepository
    {
        public RoundRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public IEnumerable<Round> GetAllRounds()
        {
            return FindAll().OrderBy(o => o.Id);
        }

        public void CreateRound(Round round)
        {
            round.Id = Guid.NewGuid();
            Create(round);
            Save();
        }

        public Round GetByRoundId(Guid id)
        {
            return FindByCondition(t => t.Id == id).Single();
        }
    }
}
