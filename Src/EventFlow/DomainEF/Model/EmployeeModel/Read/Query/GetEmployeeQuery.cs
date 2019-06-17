using System.Collections.Generic;
using System.Linq;
using EventFlow.Queries;

namespace DomainEF.Model.EmployeeModel.Read.Query
{
    public class GetEmployeeQuery : IQuery<IReadOnlyCollection<Employee>>
    {
        public GetEmployeeQuery(
            params EmployeeId[] cargoIds)
            : this((IEnumerable<EmployeeId>) cargoIds)
        {
        }

        public GetEmployeeQuery(IEnumerable<EmployeeId> cargoIds)
        {
            CargoIds = cargoIds.ToList();
        }

        public IReadOnlyCollection<EmployeeId> CargoIds { get; }
    }
}