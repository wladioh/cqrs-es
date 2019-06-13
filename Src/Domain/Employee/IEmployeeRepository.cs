using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Base;

namespace Domain.Employee
{
    public interface IEmployeeRepository : IBaseRepository<EmployeeRM>
    {
        Task<IEnumerable<EmployeeRM>> GetAll(CancellationToken tc = default);
    }
}