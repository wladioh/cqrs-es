using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Base;
using Domain.Employee;

namespace Domain.Location
{
    public interface ILocationRepository : IBaseRepository<LocationRM>
    {
        Task<IEnumerable<LocationRM>> GetAll(CancellationToken tc = default);
        Task<IEnumerable<EmployeeRM>> GetEmployees(Guid locationId, CancellationToken tc = default);
        Task<bool> HasEmployee(Guid locationId, Guid employeeId, CancellationToken tc = default);
    }
}