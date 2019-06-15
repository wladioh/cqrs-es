using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventFlow.Aggregates;
using EventFlow.Entities;
using EventFlow.Queries;
using EventFlow.ReadStores;
using EventFlow.ReadStores.InMemory;
using Mapster;

namespace DomainEF.Model.EmployeeModel
{
    public class Employee : Entity<EmployeeId>
    {
        public string FirstName { get; }
        public string LastName { get; }
        public DateTime DateOfBirth { get; }
        public string JobTitle { get; }

        public Employee(EmployeeId id, string firstName, string lastName, DateTime dateOfBirth, string jobTitle)
            : base(id)
        {
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            JobTitle = jobTitle;
        }
    }

    public class GetEmployeeQuery : IQuery<IReadOnlyCollection<Employee>>
    {
        public GetEmployeeQuery(
            params EmployeeId[] cargoIds)
            : this((IEnumerable<EmployeeId>) cargoIds)
        {
        }

        public GetEmployeeQuery(IEnumerable<EmployeeId> cargoIds)
        {
            CargoIds = cargoIds.ToList();
        }

        public IReadOnlyCollection<EmployeeId> CargoIds { get; }
    }

    public class EmployeeReadModel : IReadModel,
        IAmAsyncReadModelFor<EmployeeAggregate, EmployeeId, EmployeeCreatedEvent>
    {
        public string FirstName { set; get; }
        public string LastName { set; get; }
        public DateTime DateOfBirth { set; get; }
        public string JobTitle { set; get; }


        public Task ApplyAsync(IReadModelContext context, IDomainEvent<EmployeeAggregate, EmployeeId, EmployeeCreatedEvent> domainEvent, CancellationToken cancellationToken)
        {
            domainEvent.AggregateEvent.Adapt(this);
            return Task.CompletedTask;
        }
    }

    public class GetAllEmployee : IQuery<IReadOnlyCollection<EmployeeReadModel>>
    {
        public GetAllEmployee()
        {
        }
    }

    public class GetUserByUsernameQueryHandler :
        IQueryHandler<GetAllEmployee, IReadOnlyCollection<EmployeeReadModel>>
    {
        private IEmployeeRepository _userReadModelRepository;

        public GetUserByUsernameQueryHandler(
            IEmployeeRepository readStore)
        {
            _userReadModelRepository = readStore;
        }

        public async Task<IReadOnlyCollection<EmployeeReadModel>> ExecuteQueryAsync(GetAllEmployee query, CancellationToken cancellationToken)
        {
            return await _userReadModelRepository.GetAll(cancellationToken);
        }
    }
}