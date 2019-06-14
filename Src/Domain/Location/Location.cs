using System;
using System.Collections.Generic;
using CQRSlite.Domain;
using Domain.Location.Events;

namespace Domain.Location
{
    public class Location : AggregateRoot
    {
        private string _streetAddress;
        private string _city;
        private string _state;
        private string _postalCode;
        private readonly List<Guid> _employees = new List<Guid>();

        protected Location() { }

        public Location(Guid id, string streetAddress, string city, string state, string postalCode)
        {
            ApplyChange(new LocationCreatedEvent(id, streetAddress, city, state, postalCode));
        }

        public void AddEmployee(Guid employeeId)
        {
            ApplyChange(new EmployeeAssignedToLocationEvent(Id, employeeId));
        }

        public void RemoveEmployee(Guid employeeId)
        {
            ApplyChange(new EmployeeRemovedFromLocationEvent(Id, employeeId));
        }

        private void Apply(LocationCreatedEvent e)
        {
            Id = e.Id;
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