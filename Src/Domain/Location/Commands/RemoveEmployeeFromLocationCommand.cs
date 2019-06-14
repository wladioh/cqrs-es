using System;
using Domain.Base.Commands;

namespace Domain.Location.Commands
{
    public class RemoveEmployeeFromLocationCommand : BaseCommand
    {
        public readonly Guid EmployeeId;
        public readonly Guid LocationId;

        public RemoveEmployeeFromLocationCommand(Guid id, Guid locationId, Guid employeeId)
        {
            Id = id;
            EmployeeId = employeeId;
            LocationId = locationId;
        }
    }
}