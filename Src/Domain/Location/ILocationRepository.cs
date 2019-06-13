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
        Task<IEnumerable<EmployeeRM>> GetEmployees(int locationId, CancellationToken tc = default);
        Task<bool> HasEmployee(int locationId, int employeeId, CancellationToken tc = default);
    }
}