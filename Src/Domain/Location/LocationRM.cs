using System;
using System.Collections.Generic;
using Domain.Base;

namespace Domain.Location
{
    public class LocationRM : IEntity
    {
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public List<int> Employees { get; set; } = new List<int>();
        public Guid AggregateId { get; set; }
        public int Id { get; set; }
    }
}