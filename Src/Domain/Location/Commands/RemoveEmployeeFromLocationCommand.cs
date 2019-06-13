using System;
using Domain.Base.Commands;

namespace Domain.Location.Commands
{
    public class RemoveEmployeeFromLocationCommand : BaseCommand
    {
        public readonly int EmployeeId;
        public readonly int LocationId;

        public RemoveEmployeeFromLocationCommand(Guid id, int locationId, int employeeId)
        {
            Id = id;
            EmployeeId = employeeId;
            LocationId = locationId;
        }
    }
}