using Domain.Employee;
using StackExchange.Redis;

namespace Infra
{
    public class EmployeeRepository : BaseRepository<EmployeeRM>, IEmployeeRepository
    {
        public EmployeeRepository(IConnectionMultiplexer redisConnection) : base(redisConnection, "employee") { }
    }
}