using System;
using System.Threading;
using System.Threading.Tasks;
using DomainEF.Model.EmployeeModel.Write;
using EventFlow.Aggregates;
using EventFlow.ReadStores;
using Mapster;

namespace DomainEF.Model.EmployeeModel.Read
{
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
}