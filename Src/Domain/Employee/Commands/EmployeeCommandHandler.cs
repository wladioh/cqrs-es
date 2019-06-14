using System.Threading;
using System.Threading.Tasks;
using CQRSlite.Commands;
using CQRSlite.Domain;

namespace Domain.Employee.Commands
{
    public class EmployeeCommandHandler : ICancellableCommandHandler<CreateEmployeeCommand>
    {
        private readonly ISession _session;

        public EmployeeCommandHandler(ISession session)
        {
            _session = session;
        }

        public async Task Handle(CreateEmployeeCommand command, CancellationToken  ct)
        {
            var employee = new Employee(command.Id, command.FirstName, command.LastName, command.DateOfBirth, command.JobTitle);
            await _session.Add(employee, ct);
            await _session.Commit(ct);
        }
    }
}