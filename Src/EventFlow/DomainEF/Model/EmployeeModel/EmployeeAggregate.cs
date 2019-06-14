using System;
using EventFlow.Aggregates;

namespace DomainEF.Model.EmployeeModel
{
    public class EmployeeAggregate : AggregateRoot<EmployeeAggregate, EmployeeId>, IApply<EmployeeCreatedEvent>
    {
        private string _firstName;
        private string _lastName;
        private DateTime _dateOfBirth;
        private string _jobTitle;

        public EmployeeAggregate(EmployeeId id) : base(id)
        {
        }

        public void SetBasicInformation(string firstName, string lastName, DateTime dateOfBirth, string jobTitle)
        {
            Emit(new EmployeeCreatedEvent(Id, firstName, lastName, dateOfBirth, jobTitle));
        }

        public void Apply(EmployeeCreatedEvent aggregateEvent)
        {
            _firstName = aggregateEvent.FirstName;
            _lastName = aggregateEvent.LastName;
            _dateOfBirth = aggregateEvent.DateOfBirth;
            _jobTitle = aggregateEvent.JobTitle;
        }
    }

}