using System;
using EventFlow.Aggregates;
using EventFlow.EventStores;

namespace DomainEF.Model.EmployeeModel
{
    [EventVersion("EmployeeCreated", 1)]
    public class EmployeeCreatedEvent : IAggregateEvent<EmployeeAggregate, EmployeeId>
    {
        public readonly EmployeeId EmployeeId;
        public readonly string FirstName;
        public readonly string LastName;
        public readonly DateTime DateOfBirth;
        public readonly string JobTitle;

        public EmployeeCreatedEvent(EmployeeId employeeId, string firstName, string lastName, DateTime dateOfBirth, string jobTitle)
        {
            EmployeeId = employeeId;
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            JobTitle = jobTitle;
        }
    }
}