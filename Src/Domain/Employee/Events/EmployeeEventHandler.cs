using System.Threading;
using System.Threading.Tasks;
using CQRSlite.Commands;
using CQRSlite.Domain;
using CQRSlite.Events;
using Domain.Base;
using Domain.Location.Events;

namespace Domain.Employee.Events
{
    public class EmployeeEventHandler : ICancellableEventHandler<EmployeeCreatedEvent>
    {
        private readonly IMapper _mapper;
        private readonly IEmployeeRepository _employeeRepo;
        private readonly ISession _session;

        public EmployeeEventHandler(IMapper mapper, IEmployeeRepository employeeRepo, ISession session)
        {
            _mapper = mapper;
            _employeeRepo = employeeRepo;
            _session = session;
        }

        public async Task Handle(EmployeeCreatedEvent message, CancellationToken ct)
        {
            var employee = await _mapper.Map<EmployeeRM>(message);
            await _employeeRepo.Save(employee, ct);
        }
    }
}