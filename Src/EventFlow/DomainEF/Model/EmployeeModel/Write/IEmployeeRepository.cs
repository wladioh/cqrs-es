using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DomainEF.Model.EmployeeModel.Read;
using EventFlow.ReadStores;

namespace DomainEF.Model.EmployeeModel.Write
{
    public interface IEmployeeRepository: IReadModelStore<EmployeeReadModel>
    {
        Task<EmployeeReadModel> GetById(Guid id, CancellationToken ct = default);
        Task<IReadOnlyCollection<EmployeeReadModel>> GetAll(CancellationToken ct = default);
    }
}