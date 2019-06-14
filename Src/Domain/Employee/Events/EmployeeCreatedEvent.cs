using System;
using Domain.Base.Events;

namespace Domain.Employee.Events
{
    public class EmployeeCreatedEvent : BaseEvent
    {
        public readonly string FirstName;
        public readonly string LastName;
        public readonly DateTime DateOfBirth;
        public readonly string JobTitle;

        public EmployeeCreatedEvent(Guid employeeId, string firstName, string lastName, DateTime dateOfBirth, string jobTitle)
        {
            Id = employeeId;
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            JobTitle = jobTitle;
        }
    }
}