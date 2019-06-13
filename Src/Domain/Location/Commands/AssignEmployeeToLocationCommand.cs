using System;
using Domain.Base.Commands;

namespace Domain.Location.Commands
{
    public class AssignEmployeeToLocationCommand : BaseCommand
    {
        public readonly int EmployeeId;
        public readonly int LocationId;

        public AssignEmployeeToLocationCommand(Guid id, int locationId, int employeeId)
        {
            Id = id;
            EmployeeId = employeeId;
            LocationId = locationId;
        }
    }
}