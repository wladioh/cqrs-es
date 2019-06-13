using System;
using Domain.Base;

namespace Domain.Employee
{
    public class EmployeeRM: IEntity
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string JobTitle { get; set; }
        public int LocationId { get; set; }
        public Guid AggregateId { get; set; }
        public int Id { get; set; }
    }
}