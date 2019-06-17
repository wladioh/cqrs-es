using System.Collections.Generic;
using DomainEF.Model.EmployeeModel;
using EventFlow.Aggregates;
using EventFlow.EventStores;

namespace DomainEF.Model.Location
{
    public class LocationAggregate : AggregateRoot<LocationAggregate, LocationId>, 
        IApply<LocationCreatedEvent>,
        IApply<EmployeeAssignedToLocationEvent>,
        IApply<EmployeeRemovedFromLocationEvent>
    {
        private string _streetAddress;
        private string _city;
        private string _state;
        private string _postalCode;
        private readonly List<EmployeeId> _employees = new List<EmployeeId>();
        public LocationAggregate(LocationId id) : base(id)
        {
        }

        public void Initialize(string streetAddress, string city, string state, string postalCode)
        {
            Emit(new LocationCreatedEvent(Id, streetAddress, city, state, postalCode));
        }

        public void AddEmployee(EmployeeId employeeId)
        {
            Emit(new EmployeeAssignedToLocationEvent(employeeId, Id));
        }

        public void RemoveEmployee(EmployeeId employeeId)
        {
            Emit(new EmployeeRemovedFromLocationEvent(Id, employeeId));
        }

        public void Apply(LocationCreatedEvent e)
        {
            _streetAddress = e.StreetAddress;
            _city = e.City;
            _state = e.State;
            _postalCode = e.PostalCode;
        }

        public void Apply(EmployeeRemovedFromLocationEvent e)
        {
            _employees.Remove(e.EmployeeId);
        }

        public void Apply(EmployeeAssignedToLocationEvent e)
        {
            _employees.Add(e.EmployeeId);
        }
    }

    [EventVersion("EmployeeAssignedToLocation", 1)]
    public class EmployeeAssignedToLocationEvent : IAggregateEvent<LocationAggregate, LocationId>
    {
        public EmployeeId EmployeeId { get; }
        public LocationId LocationId { get; }

        public EmployeeAssignedToLocationEvent(EmployeeId employeeId, LocationId locationId)
        {
            EmployeeId = employeeId;
            LocationId = locationId;
        }
    }
    [EventVersion("LocationCreated", 1)]
    public class LocationCreatedEvent : IAggregateEvent<LocationAggregate, LocationId>
    {
        public LocationId LocationId { get; }
        public string StreetAddress { get; }
        public string City { get; }
        public string State { get; }
        public string PostalCode { get; }

        public LocationCreatedEvent(LocationId locationId, string streetAddress, string city, string state, string postalCode)
        {
            LocationId = locationId;
            StreetAddress = streetAddress;
            City = city;
            State = state;
            PostalCode = postalCode;
        }
    }

    [EventVersion("EmployeeRemovedFromLocation", 1)]
    public class EmployeeRemovedFromLocationEvent : IAggregateEvent<LocationAggregate, LocationId>
    {
        public LocationId LocationId { get; }
        public EmployeeId EmployeeId { get; }

        public EmployeeRemovedFromLocationEvent(LocationId locationId, EmployeeId employeeId)
        {
            LocationId = locationId;
            EmployeeId = employeeId;
        }
    }
}