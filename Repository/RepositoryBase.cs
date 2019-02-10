using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Contracts;
using Entities;

namespace Repository
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class 
    {
        protected RepositoryContext RepositoryContext { get; set; }

        public RepositoryBase(RepositoryContext repositoryContext)
        {
            this.RepositoryContext = repositoryContext;
        }

        public IEnumerable<T> FindAll()
        {
            return this.RepositoryContext.Set<T>();
        }

        public void Create(T entity)
        {
            this.RepositoryContext.Set<T>().Add(entity);
        }

        public void Save()
        {
            this.RepositoryContext.SaveChanges();
        }

        public IEnumerable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return this.RepositoryContext.Set<T>().Where(expression);
        }
    }
}
