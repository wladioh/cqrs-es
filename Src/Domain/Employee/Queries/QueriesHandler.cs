using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CQRSlite.Queries;

namespace Domain.Employee.Queries
{
    public class QueriesHandler : ICancellableQueryHandler<GetAllEmployees, IEnumerable<EmployeeRM>>,
        ICancellableQueryHandler<GetEmployeeById, EmployeeRM>,
        ICancellableQueryHandler<IsEmployeeAlreadyAssignedToLocation, bool>,
        ICancellableQueryHandler<EmployeeExists, bool>
    {
        private readonly IEmployeeRepository _employeeRepository;

        public QueriesHandler(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }
        public async Task<IEnumerable<EmployeeRM>> Handle(GetAllEmployees message, CancellationToken token = default)
        {
            return await _employeeRepository.GetAll(token);
        }

        public async Task<EmployeeRM> Handle(GetEmployeeById message, CancellationToken token = default)
        {
            return await _employeeRepository.GetById(message.Id, token);
        }


        public async Task<bool> Handle(EmployeeExists message, CancellationToken token = default)
        {
            return await _employeeRepository.Exists(message.Id, token);
        }

        public async Task<bool> Handle(IsEmployeeAlreadyAssignedToLocation message, CancellationToken token = default)
        {
            var employee = await _employeeRepository.GetById(message.EmployeeId, token);
            return employee.LocationId == message.LocationId;
        }
    }
    public class GetAllEmployees : IQuery<IEnumerable<EmployeeRM>>
    {

    }

    public class EmployeeExists : IQuery<bool>
    {
        public Guid Id { get; }

        public EmployeeExists(Guid id)
        {
            Id = id;
        }
    }
    public class GetEmployeeById : IQuery<EmployeeRM>
    {
        public Guid Id { get; }

        public GetEmployeeById(Guid id)
        {
            Id = id;
        }
    }

    public class IsEmployeeAlreadyAssignedToLocation: IQuery<bool>
    {
        public Guid EmployeeId { get; }
        public Guid LocationId { get; }

        public IsEmployeeAlreadyAssignedToLocation(Guid employeeId, Guid locationId)
        {
            EmployeeId = employeeId;
            LocationId = locationId;
        }
    }
}