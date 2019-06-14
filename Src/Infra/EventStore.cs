using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CQRSlite.Events;
using EventStore.ClientAPI;
using Newtonsoft.Json;

namespace Infra
{
    public class EventStore : IEventStore
    {
        private readonly IEventPublisher _publisher;
        private readonly IEventStoreConnection _connection;


        public EventStore(IEventPublisher publisher, IEventStoreConnection connection)
        {
            _publisher = publisher;
            _connection = connection;
            _connection.ConnectAsync().Wait();
        }

        public async Task Save(IEnumerable<IEvent> events, CancellationToken cancellationToken = default)
        {

            foreach (var @event in events)
            {
                await SaveAndNotify(cancellationToken, @event);
            }
        }

        private async Task SaveAndNotify(CancellationToken cancellationToken, IEvent @event)
        {
            var currentVersion = @event.Version - 2;
            var expectedVersion =@event.Version - 1;
            var t = await _connection.StartTransactionAsync(@event.Id.ToString(),  expectedVersion);
            try
            {
                await _connection.AppendToStreamAsync(@event.Id.ToString(), currentVersion,
                    CreateEventData(@event));
                await _publisher.Publish(@event, cancellationToken);
                await t.CommitAsync();
            }
            catch (Exception)
            {
                t.Rollback();
            }
            finally
            {
                t.Dispose();
            }
        }

        private static EventData CreateEventData(IEvent @event)
        {
            return new EventData(@event.Id, @event.GetType().AssemblyQualifiedName, true,
                Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(@event)), new byte[0]);
        }


        private static Assembly AssemblyResolver(AssemblyName assemblyName)
        {
            assemblyName.Version = null;
            return Assembly.Load(assemblyName);
        }
        public async Task<IEnumerable<IEvent>> Get(Guid aggregateId, int fromVersion,
            CancellationToken cancellationToken = default)
        {
            StreamEventsSlice stream;
            var events = new List<IEvent>(100);
            do
            {
                stream = await _connection.ReadStreamEventsForwardAsync(aggregateId.ToString(), fromVersion < 0 ? 0 : fromVersion, 100, true);
                foreach (var resolvedEvent in stream.Events)
                {
                    var data = Encoding.UTF8.GetString(resolvedEvent.Event.Data);
                    var type = Type.GetType(resolvedEvent.Event.EventType, AssemblyResolver, null);

                    events.Add((IEvent)JsonConvert.DeserializeObject(data, type));
                }

            }
            while (!stream.IsEndOfStream);

            return events;
        }
    }
}