using System.Threading;
using System.Threading.Tasks;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;

namespace DomainEF.Model.EmployeeModel.Write.Commands
{
    public class EmployeeCommandHandler :
        CommandHandler<EmployeeAggregate, EmployeeId, IExecutionResult, CreateEmployeeCommand>
    {
        public override Task<IExecutionResult> ExecuteCommandAsync(
            EmployeeAggregate aggregate,
            CreateEmployeeCommand command,
            CancellationToken cancellationToken)
        {
            aggregate.SetBasicInformation(
                command.FirstName, command.LastName, command.DateOfBirth, command.JobTitle);

            return Task.FromResult(ExecutionResult.Success());
        }
    }
}