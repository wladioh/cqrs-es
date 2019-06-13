using System;
using Domain.Base.Commands;

namespace Domain.Employee.Commands
{
    public class CreateEmployeeCommand : BaseCommand
    {
        public readonly int EmployeeId;
        public readonly string FirstName;
        public readonly string LastName;
        public readonly DateTime DateOfBirth;
        public readonly string JobTitle;
        public CreateEmployeeCommand(Guid id, int employeeId, string firstName, string lastName, DateTime dateOfBirth, string jobTitle)
        {
            Id = id;
            EmployeeId = employeeId;
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            JobTitle = jobTitle;
        }
    }
}