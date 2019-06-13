using System;
using System.Collections.Generic;
using CQRSlite.Domain;
using Domain.Location.Events;

namespace Domain.Location
{
    public class Location : AggregateRoot
    {
        private int _locationId;
        private string _streetAddress;
        private string _city;
        private string _state;
        private string _postalCode;
        private readonly List<int> _employees = new List<int>();

        protected Location() { }

        public Location(Guid id, int locationId, string streetAddress, string city, string state, string postalCode)
        {
            ApplyChange(new LocationCreatedEvent(id, locationId, streetAddress, city, state, postalCode));
        }

        public void AddEmployee(int employeeId)
        {
            ApplyChange(new EmployeeAssignedToLocationEvent(Id, _locationId, employeeId));
        }

        public void RemoveEmployee(int employeeId)
        {
            ApplyChange(new EmployeeRemovedFromLocationEvent(Id, _locationId, employeeId));
        }

        private void Apply(LocationCreatedEvent e)
        {
            Id = e.Id;
            _locationId = e.LocationId;
            _streetAddress = e.StreetAddress;
            _city = e.City;
            _state = e.State;
            _postalCode = e.PostalCode;
        }

        private void Apply(EmployeeRemovedFromLocationEvent e)
        {
            _employees.Remove(e.EmployeeId);
        }

        private void Apply(EmployeeAssignedToLocationEvent e)
        {
            _employees.Add(e.EmployeeId);
        }
    }
}