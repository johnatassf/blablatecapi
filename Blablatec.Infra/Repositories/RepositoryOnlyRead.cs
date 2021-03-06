﻿using Blablatec.Domain.Interface;
using Blablatec.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Blablatec.Infra.Repositories
{
    public class RepositoryOnlyRead<T>: IRepositoryOnlyRead<T> where T : class, IEntity
    {
        private readonly DbSet<T> _dbSet;

        public RepositoryOnlyRead(ContextBlablatec contextBlablatec)
        {
            _dbSet = contextBlablatec.Set<T>();
        }

        public bool Exists(int id)
        {
            return _dbSet.AsNoTracking().Any(t => t.Id == id);
        }

        public List<T> GetAll()
        {
            return _dbSet.ToList();
        }
        public List<T> GetAll(Expression<Func<T, bool>> expression)
        {
            return _dbSet.Where(expression).ToList();
        }
        public async Task<T> GetOne(Expression<Func<T, bool>> expression)
        {
            return await _dbSet.FirstOrDefaultAsync(expression);
        }

        public T GetById(int id, params Expression<Func<T, object>>[] includes)
        {
            var queryWithIncludes = includes.Aggregate(_dbSet.AsQueryable(),
                (query, include) => query.Include(include));

            return queryWithIncludes.FirstOrDefault(entity => entity.Id == id);
        }

        public List<T> GetEntityByExpression(Expression<Func<T, bool>> expression = null, params Expression<Func<T, object>>[] includes)
        {
            var lista = ObterTodosComInclude(includes);
            if(expression != null)
            lista = lista.Where(expression);

            return lista.AsNoTracking().ToList();
        }

        private IQueryable<T> ObterTodosComInclude(params Expression<Func<T, object>>[] includes)
        {
            return includes.Aggregate(_dbSet.AsQueryable().AsNoTracking(),
                (consulta, include) => consulta.Include(include));
        }

    }
}
