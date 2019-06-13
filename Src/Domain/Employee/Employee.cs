using System;
using CQRSlite.Domain;
using Domain.Employee.Events;

namespace Domain.Employee
{
    public class Employee : AggregateRoot
    {
        private int _employeeId;
        private string _firstName;
        private string _lastName;
        private DateTime _dateOfBirth;
        private string _jobTitle;

        protected Employee() { }

        public Employee(Guid id, int employeeId, string firstName, string lastName, DateTime dateOfBirth, string jobTitle)
        {
            Id = id;
            _employeeId = employeeId;
            _firstName = firstName;
            _lastName = lastName;
            _dateOfBirth = dateOfBirth;
            _jobTitle = jobTitle;

            ApplyChange(new EmployeeCreatedEvent(id, employeeId, firstName, lastName, dateOfBirth, jobTitle));
        }
    }
}