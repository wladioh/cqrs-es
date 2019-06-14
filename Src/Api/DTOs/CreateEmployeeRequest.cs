using System;

namespace Api.DTOs
{
    public class CreateEmployeeRequest
    {
        public Guid LocationId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string JobTitle { get; set; }
    }
}