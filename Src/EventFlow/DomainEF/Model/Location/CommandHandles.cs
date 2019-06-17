using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;

namespace DomainEF.Model.Location
{
    public class CommandHandles :
        CommandHandler<LocationAggregate, LocationId, IExecutionResult, CreateLocationCommand>
    {
        public override Task<IExecutionResult> ExecuteCommandAsync(LocationAggregate aggregate, CreateLocationCommand command,
            CancellationToken cancellationToken)
        {
            aggregate.Initialize(command.StreetAddress, command.City, command.State, command.PostalCode);
            return Task.FromResult(ExecutionResult.Success());
        }
    }

    public class CreateLocationCommand : DistinctCommand<LocationAggregate, LocationId, IExecutionResult>
    {
        public readonly string StreetAddress;
        public readonly string City;
        public readonly string State;
        public readonly string PostalCode;

        public CreateLocationCommand(LocationId id, string streetAddress, string city, string state, string postalCode) : base(id)
        {
            StreetAddress = streetAddress;
            City = city;
            State = state;
            PostalCode = postalCode;
        }

        protected override IEnumerable<byte[]> GetSourceIdComponents()
        {
            yield return Encoding.UTF8.GetBytes(City);
            yield return Encoding.UTF8.GetBytes(StreetAddress);
            yield return Encoding.UTF8.GetBytes(State);
            yield return Encoding.UTF8.GetBytes(PostalCode);
        }
    }
}