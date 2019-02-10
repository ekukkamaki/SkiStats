using System;
using System.Collections.Generic;
using System.Text;
using Entities.Models;

namespace Contracts
{
    public interface IPersonRepository
    {
        void AddPerson(Person person);
        Person GetById(Guid id);
    }
}