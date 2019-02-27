using System;
using System.Net.Http;
using System.Runtime.Serialization;
using Contracts;
using Entities;
using Entities.Enumerations;
using Entities.Models;
using ExternalServices.Fmi;
using ExternalServices.Fmi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace SkiApiServer.Controllers
{
    [Route("api/round")]
    [ApiController]
    public class RoundController : ControllerBase
    {
        private readonly IRepositoryWrapper _repository;
        private readonly FmiService _fmiService;

        public RoundController(IRepositoryWrapper repo, FmiService fmiService)
        {
            _repository = repo;
            _fmiService = fmiService;
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

        public class RoundArgs : Round
        {
            [IgnoreDataMember]
            public Guid PersonId { get; set; }
            [IgnoreDataMember]
            public Guid LocationId { get; set; }
        }

        [HttpPost]
        public IActionResult AddRound([FromBody] RoundArgs round)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid object");
                }
                
                round.Person = _repository.Person.GetById(round.PersonId);
                if (round.Person == null)
                    return NotFound("Person not found");

                round.Location = _repository.Location.GetById(round.LocationId);

                _repository.Round.CreateRound(round);
                
                return CreatedAtRoute("getByRoundId", new { id = round.Id }, round);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        public class WeatherApiRequest
        {
            public string city { get; set; }
            public DateTime startTime { get; set; }
            public DateTime endTime { get; set; }
        }

        [HttpGet("test")]
        public IActionResult GetFeature([FromQuery] WeatherApiRequest req)
        {
            var features = new FeatureRepository(_fmiService).GetSnowDepthAndTemperature(req.city,req.startTime,req.endTime);
            return Ok(features);
        }
    }
}
