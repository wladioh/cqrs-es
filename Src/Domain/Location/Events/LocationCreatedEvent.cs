using System;
using Domain.Base.Events;

namespace Domain.Location.Events
{
    public class LocationCreatedEvent : BaseEvent
    {
        public readonly string StreetAddress;
        public readonly string City;
        public readonly string State;
        public readonly string PostalCode;

        public LocationCreatedEvent(Guid id, string streetAddress, string city, string state, string postalCode)
        {
            Id = id;
            StreetAddress = streetAddress;
            City = city;
            State = state;
            PostalCode = postalCode;
        }
    }
}