using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Base
{
    public interface IBaseRepository<T> where T : IEntity
    {
        Task<T> GetById(int id, CancellationToken tc = default);
        Task<List<T>> GetMultiple(int[] ids, CancellationToken tc = default);
        Task<bool> Exists(int id, CancellationToken tc = default);
        Task Save(T item, CancellationToken tc = default);
    }

    public interface IEntity
    {
        int Id { get; set;  }
    }
}