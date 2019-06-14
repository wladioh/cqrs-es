using System;

namespace Api.DTOs
{
    public class AssignEmployeeToLocationRequest
    {
        public Guid LocationId { get; set; }
        public Guid EmployeeId { get; set; }
    }
}