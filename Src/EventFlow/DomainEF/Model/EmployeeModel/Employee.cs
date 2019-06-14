using System;
using System.Collections.Generic;
using System.Linq;
using EventFlow.Entities;
using EventFlow.Queries;

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

    public class GetEmployeeQuery : IQuery<IReadOnlyCollection<Employee>>
    {
        public GetEmployeeQuery(
            params EmployeeId[] cargoIds)
            : this((IEnumerable<EmployeeId>) cargoIds)
        {
        }

        public GetEmployeeQuery(IEnumerable<EmployeeId> cargoIds)
        {
            CargoIds = cargoIds.ToList();
        }

        public IReadOnlyCollection<EmployeeId> CargoIds { get; }
    }
}