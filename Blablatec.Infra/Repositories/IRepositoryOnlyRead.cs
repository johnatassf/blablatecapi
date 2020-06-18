using Blablatec.Domain.Interface;
using Blablatec.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Blablatec.Infra.Repositories
{
    public interface IRepositoryOnlyRead<T> where T : class, IEntity
    {

        T GetById(int id, params Expression<Func<T, object>>[] includes);

        List<T> GetEntityByExpression(Expression<Func<T, bool>> expression = null, params Expression<Func<T, object>>[] includes);

        List<T> GetAll();

        List<T> GetAll(Expression<Func<T, bool>> expression);

        Task<T> GetOne(Expression<Func<T, bool>> expression);

        bool Exists(int id);

    }
}