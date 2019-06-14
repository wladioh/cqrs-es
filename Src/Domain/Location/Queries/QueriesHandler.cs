using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CQRSlite.Queries;
using Domain.Employee;

namespace Domain.Location.Queries
{
    public class QueriesHandler : ICancellableQueryHandler<GetAllLocations, IReadOnlyCollection<LocationRM>>,
        ICancellableQueryHandler<GetLocationById, LocationRM>,
        ICancellableQueryHandler<LocationExists, bool>,
        ICancellableQueryHandler<GetEmployeesFromLocation, IList<EmployeeRM>>
    {
        private readonly ILocationRepository _locationRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public QueriesHandler(ILocationRepository locationRepository, IEmployeeRepository employeeRepository)
        {
            _locationRepository = locationRepository;
            _employeeRepository = employeeRepository;
        }
        public async Task<IReadOnlyCollection<LocationRM>> Handle(GetAllLocations message, CancellationToken token = default)
        {
            return (await _locationRepository.GetAll(token)).ToList();
        }

        public async Task<LocationRM> Handle(GetLocationById message, CancellationToken token = default)
        {
            return await _locationRepository.GetById(message.LocationId, token);
        }

        public async Task<bool> Handle(LocationExists message, CancellationToken token = new CancellationToken())
        {
            return await _locationRepository.Exists(message.LocationId, token);
        }

        public async Task<IList<EmployeeRM>> Handle(GetEmployeesFromLocation message, CancellationToken token = new CancellationToken())
        {
            var location = await _locationRepository.GetById(message.LocationId, token);

            return await _employeeRepository.GetMultiple(location.Employees.ToArray(), token);
        }
    }

    public class GetAllLocations : IQuery<IReadOnlyCollection<LocationRM>>
    {

    }
    public class GetLocationById : IQuery<LocationRM>
    {
        public Guid LocationId { get; }

        public GetLocationById(Guid locationId)
        {
            LocationId = locationId;
        }
    }

    public class LocationExists : IQuery<bool>
    {
        public Guid LocationId { get; }
        public LocationExists(Guid locationId)
        {
            LocationId = locationId;
        }
    }

    public class GetEmployeesFromLocation : IQuery<IList<EmployeeRM>>
    {
        public Guid LocationId { get; }
        public GetEmployeesFromLocation(Guid locationId)
        {
            LocationId = locationId;
        }
    }
}