using Blablatec.Domain.Interface;
using Blablatec.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blablatec.Infra.Repositories
{
    public interface IRepository<T> : IRepositoryOnlyRead<T> where T : class, IEntity
    {
        void Remove(T entity);

        T Save(T entity);

        T Update(T entity);
    }
}
