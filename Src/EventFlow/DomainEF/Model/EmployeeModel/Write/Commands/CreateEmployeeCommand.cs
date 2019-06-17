using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;

namespace DomainEF.Model.EmployeeModel.Write.Commands
{
    public class CreateEmployeeCommand : DistinctCommand<EmployeeAggregate, EmployeeId, IExecutionResult>
    {
        public readonly string FirstName;
        public readonly string LastName;
        public readonly DateTime DateOfBirth;
        public readonly string JobTitle;
        public CreateEmployeeCommand(EmployeeId employeeId, string firstName, string lastName, DateTime dateOfBirth, string jobTitle)
            : base(employeeId)
        {
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            JobTitle = jobTitle;
        }

        protected override IEnumerable<byte[]> GetSourceIdComponents()
        {
            yield return Encoding.UTF8.GetBytes(FirstName);
            yield return Encoding.UTF8.GetBytes(LastName);
            yield return Encoding.UTF8.GetBytes(DateOfBirth.ToString(CultureInfo.InvariantCulture));
            yield return Encoding.UTF8.GetBytes(JobTitle);
        }
    }
}