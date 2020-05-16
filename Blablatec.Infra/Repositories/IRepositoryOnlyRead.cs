using Blablatec.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Blablatec.Infra.Repositories
{
    public interface IRepositoryOnlyRead<T> where T : class, IEntity
    {

        T GetById(int id, params Expression<Func<T, object>>[] includes);

        T GetEntityByExpression(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includes);

        List<T> GetAll();

        List<T> GetAll(Expression<Func<T, bool>> expression);

    }
}