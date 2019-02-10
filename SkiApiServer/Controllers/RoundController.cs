using System;
using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;

namespace SkiApiServer.Controllers
{
    [Route("api/round")]
    [ApiController]
    public class RoundController : ControllerBase
    {
        private readonly IRepositoryWrapper _repository;

        public RoundController(IRepositoryWrapper repo)
        {
            _repository = repo;
        }
        [HttpGet]
        public IActionResult GetRounds()
        {
            try
            {
                return Ok(_repository.Round.GetAllRounds());
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}", Name = "getByRoundId")]
        public IActionResult GetById(Guid id)
        {
            try
            {
                return Ok(_repository.Round.GetByRoundId(id));
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public IActionResult AddRound([FromBody] Round round)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid object");
                }
                
                round.Person = _repository.Person.GetById(round.Person.Id);
                if (round.Person == null)
                    return NotFound("Person not found");

                _repository.Round.CreateRound(round);

                return CreatedAtRoute("getByRoundId", new { id = round.Id }, round);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
