using System.Threading;
using System.Threading.Tasks;
using EventFlow.Aggregates;
using EventFlow.ReadStores;
using Mapster;

namespace DomainEF.Model.Location
{
    public class LocationReadModel : IReadModel,
        IAmAsyncReadModelFor<LocationAggregate, LocationId, LocationCreatedEvent>
    {
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public Task ApplyAsync(IReadModelContext context, IDomainEvent<LocationAggregate, LocationId, LocationCreatedEvent> domainEvent, CancellationToken cancellationToken)
        {
            domainEvent.AggregateEvent.Adapt(this);
            return Task.CompletedTask;
        }
    }
}