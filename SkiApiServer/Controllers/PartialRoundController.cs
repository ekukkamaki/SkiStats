using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Microsoft.AspNetCore.Mvc;

namespace SkiApiServer.Controllers
{
    public class PartialRoundController : ControllerBase
    {
        private readonly IRepositoryWrapper _repository;

        public PartialRoundController(IRepositoryWrapper repo)
        {
            _repository = repo;
        }


    }
}
