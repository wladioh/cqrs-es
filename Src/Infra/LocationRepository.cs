using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Employee;
using Domain.Location;
using StackExchange.Redis;

namespace Infra
{
    public class LocationRepository : BaseRepository<LocationRM>, ILocationRepository
    {
        public LocationRepository(IConnectionMultiplexer redis) : base(redis, "location")
        {
        }

        public async Task<IEnumerable<EmployeeRM>> GetEmployees(Guid locationId, CancellationToken tc = default)
        {
            return await Get<List<EmployeeRM>>(locationId + ":employees", tc);
        }

        public async Task<bool> HasEmployee(Guid locationId, Guid employeeId, CancellationToken tc = default)
        {
            //Deserialize the LocationDTO with the key location:{locationID}
            var location = await GetById(locationId, tc);

            //If that location has the specified Employee, return true
            return location.Employees.Contains(employeeId);
        }
    }
}