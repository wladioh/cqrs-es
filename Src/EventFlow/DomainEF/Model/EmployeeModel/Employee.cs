using System;
using EventFlow.Entities;
using EventFlow.ReadStores.InMemory;

namespace DomainEF.Model.EmployeeModel
{
    public class Employee : Entity<EmployeeId>
    {
        public string FirstName { get; }
        public string LastName { get; }
        public DateTime DateOfBirth { get; }
        public string JobTitle { get; }

        public Employee(EmployeeId id, string firstName, string lastName, DateTime dateOfBirth, string jobTitle)
            : base(id)
        {
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            JobTitle = jobTitle;
        }
    }
}