using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public interface IRepositoryBase<T>
    {
        IEnumerable<T> FindAll();
        void Create(T entity);
    }
}
