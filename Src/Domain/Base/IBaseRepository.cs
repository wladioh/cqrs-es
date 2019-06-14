using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Base
{
    public interface IBaseRepository<T> where T : IEntity
    {
        Task<T> GetById(Guid id, CancellationToken tc = default);
        Task<List<T>> GetMultiple(Guid[] ids, CancellationToken tc = default);
        Task<bool> Exists(Guid id, CancellationToken tc = default);
        Task Save(T item, CancellationToken tc = default);
    }

    public interface IEntity
    {
        Guid Id { get; set;  }
    }
}