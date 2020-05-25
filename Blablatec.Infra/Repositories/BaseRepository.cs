using Blablatec.Domain.Interface;
using Blablatec.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Blablatec.Infra.Repositories
{
    public class BaseRepository<T> : RepositoryOnlyRead<T>, IRepositoryOnlyRead<T>, IRepository<T> where T : class, IEntity
    {
        private readonly ContextBlablatec _contextBlablatec;
        private readonly DbSet<T> _dbSet;

        public BaseRepository(ContextBlablatec contextBlablatec): base(contextBlablatec)
        {
            _contextBlablatec = contextBlablatec;
            _dbSet = _contextBlablatec.Set<T>();
        }

      
        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
            _contextBlablatec.SaveChanges();
        }

        public T Save(T entity)
        {
            _dbSet.Add(entity);
            _contextBlablatec.SaveChanges();

            return entity;
        }

        public T Update(T entity)
        {
            _dbSet.Attach(entity);
            _contextBlablatec.Entry(entity).State = EntityState.Modified;
            _contextBlablatec.SaveChanges();
            
            return entity;
        }
    }
}
