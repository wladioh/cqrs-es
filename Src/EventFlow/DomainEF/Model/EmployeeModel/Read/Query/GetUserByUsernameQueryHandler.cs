using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DomainEF.Model.EmployeeModel.Write;
using EventFlow.Queries;

namespace DomainEF.Model.EmployeeModel.Read.Query
{
    public class GetUserByUsernameQueryHandler :
        IQueryHandler<GetAllEmployee, IReadOnlyCollection<EmployeeReadModel>>
    {
        private IEmployeeRepository _userReadModelRepository;

        public GetUserByUsernameQueryHandler(
            IEmployeeRepository readStore)
        {
            _userReadModelRepository = readStore;
        }

        public async Task<IReadOnlyCollection<EmployeeReadModel>> ExecuteQueryAsync(GetAllEmployee query, CancellationToken cancellationToken)
        {
            return await _userReadModelRepository.GetAll(cancellationToken);
        }
    }
}