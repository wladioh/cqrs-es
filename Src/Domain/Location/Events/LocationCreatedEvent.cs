using System;
using Domain.Base.Events;

namespace Domain.Location.Events
{
    public class LocationCreatedEvent : BaseEvent
    {
        public readonly int LocationId;
        public readonly string StreetAddress;
        public readonly string City;
        public readonly string State;
        public readonly string PostalCode;

        public LocationCreatedEvent(Guid id, int locationId, string streetAddress, string city, string state, string postalCode)
        {
            Id = id;
            LocationId = locationId;
            StreetAddress = streetAddress;
            City = city;
            State = state;
            PostalCode = postalCode;
        }
    }
}