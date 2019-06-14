using System;
using System.Threading;
using System.Threading.Tasks;
using CQRSlite.Events;
using Domain.Base;
using Domain.Employee;

namespace Domain.Location.Events
{
    public class LocationEventHandler : ICancellableEventHandler<LocationCreatedEvent>,
        ICancellableEventHandler<EmployeeAssignedToLocationEvent>,
        ICancellableEventHandler<EmployeeRemovedFromLocationEvent>
    {
        private readonly IMapper _mapper;
        private readonly ILocationRepository _locationRepo;
        private readonly IEmployeeRepository _employeeRepo;
        public LocationEventHandler(IMapper mapper, ILocationRepository locationRepo, IEmployeeRepository employeeRepo)
        {
            _mapper = mapper;
            _locationRepo = locationRepo;
            _employeeRepo = employeeRepo;
        }

        public async Task Handle(LocationCreatedEvent message, CancellationToken ct)
        {
            //Create a new LocationDTO object from the LocationCreatedEvent
            var location = await _mapper.Map<LocationRM>(message);

            await _locationRepo.Save(location, ct);
        }

        public async Task Handle(EmployeeAssignedToLocationEvent message, CancellationToken ct)
        {
            var location = await _locationRepo.GetById(message.Id, ct);
            location.Employees.Add(message.EmployeeId);
            await _locationRepo.Save(location, ct);

            //Find the employee which was assigned to this Location
            var employee = await _employeeRepo.GetById(message.EmployeeId, ct);
            employee.LocationId = message.Id;
            await _employeeRepo.Save(employee, ct);
        }

        public async Task Handle(EmployeeRemovedFromLocationEvent message, CancellationToken ct)
        {
            var location = await _locationRepo.GetById(message.Id, ct);
            location.Employees.Remove(message.EmployeeId);
            await _locationRepo.Save(location, ct);

            //Find the employee which was assigned to this Location
            var employee = await _employeeRepo.GetById(message.EmployeeId, ct);
            employee.LocationId = Guid.Empty;
            await _employeeRepo.Save(employee, ct);
        }
    }
}