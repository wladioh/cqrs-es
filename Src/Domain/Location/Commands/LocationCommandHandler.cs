using System.Threading;
using System.Threading.Tasks;
using CQRSlite.Commands;
using CQRSlite.Domain;

namespace Domain.Location.Commands
{
    public class LocationCommandHandler : ICancellableCommandHandler<CreateLocationCommand>,
        ICancellableCommandHandler<AssignEmployeeToLocationCommand>,
        ICancellableCommandHandler<RemoveEmployeeFromLocationCommand>
    {
        private readonly ISession _session;

        public LocationCommandHandler(ISession session)
        {
            _session = session;
        }

        public async Task Handle(CreateLocationCommand command, CancellationToken ct)
        {
            var location = new Location(command.Id, command.LocationId, command.StreetAddress, command.City, command.State, command.PostalCode);
            await _session.Add(location, ct);
            await _session.Commit(ct);
        }

        public async Task Handle(AssignEmployeeToLocationCommand command, CancellationToken ct)
        {
            var location = await _session.Get<Location>(command.Id, cancellationToken: ct);
            location.AddEmployee(command.EmployeeId);
            //await _session.Commit(ct);
        }

        public async Task Handle(RemoveEmployeeFromLocationCommand command, CancellationToken ct)
        {
            var location = await  _session.Get<Location>(command.Id, cancellationToken: ct);
            location.RemoveEmployee(command.EmployeeId);
            await _session.Commit(ct);
        }
    }
}