using System;
using System.Collections.Generic;
using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace SkiApiServer.Controllers
{
  [Route("api/person")]
  [ApiController]
  public class PersonController : ControllerBase
  {
    private readonly IRepositoryWrapper _repository;

    public PersonController(IRepositoryWrapper repo)
    {
      _repository = repo;
    }

    [HttpGet(Name = "get_all_persons")]
    public ActionResult<IEnumerable<Person>> GetAll()
    {
      try
      {
        return Ok(_repository.Person.GetAll());
      }
      catch (System.Exception)
      {
        return StatusCode(500, "Internal server error");
      }

    }

    [HttpGet("{id}", Name = "personById")]
    public ActionResult<Person> GetById(Guid id)
    {
      try
      {
        return Ok(_repository.Person.GetById(id));
      }
      catch (Exception e)
      {
        return StatusCode(500, "Internal server error");
      }
    }

    [HttpPost(Name = "post_person")]
    public IActionResult AddPerson([FromBody] Person person)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest("Invalid object");
      }
      _repository.Person.AddPerson(person);
      return CreatedAtRoute("personById", new { id = person.Id }, person);
    }

  }
}
