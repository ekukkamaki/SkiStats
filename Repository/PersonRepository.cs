using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Contracts;
using Entities;
using Entities.Models;

namespace Repository
{
  public class PersonRepository : RepositoryBase<Person>, IPersonRepository
  {
    public PersonRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {
    }

    public List<Person> GetAll()
    {
      return FindAll().ToList();
    }

    public void AddPerson(Person person)
    {
      person.Id = Guid.NewGuid();
      Create(person);
      Save();
    }
    public Person GetById(Guid id)
    {
      return FindByCondition(t => t.Id == id).Single();
    }
  }
}
