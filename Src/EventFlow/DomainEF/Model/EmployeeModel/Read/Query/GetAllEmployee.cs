using System.Collections.Generic;
using EventFlow.Queries;

namespace DomainEF.Model.EmployeeModel.Read.Query
{
    public class GetAllEmployee : IQuery<IReadOnlyCollection<EmployeeReadModel>>
    {
        public GetAllEmployee()
        {
        }
    }
}