using System;
using System.Collections.Generic;
using System.Text;
using Contracts;
using Entities;

namespace Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private IRoundRepository _roundRepository;
        private IPersonRepository _personRepository;
        private ILocationRepository _locationRepository;
        private readonly RepositoryContext _repoContext;
        


        public IRoundRepository Round
        {
            get
            {
                if (_roundRepository == null)
                {
                    _roundRepository = new RoundRepository(_repoContext);
                }

                return _roundRepository;
            }
        }

        public IPersonRepository Person
        {
            get
            {
                if (_personRepository == null)
                {
                    _personRepository = new PersonRepository(_repoContext);
                }

                return _personRepository;
            }
        }

        public ILocationRepository Location
        {
            get
            {
                if (_locationRepository == null)
                {
                    _locationRepository = new LocationRepository(_repoContext);
                }

                return _locationRepository;
            }
        }

        public RepositoryWrapper(RepositoryContext repositoryContext)
        {
            _repoContext = repositoryContext;
        }
    }
}
