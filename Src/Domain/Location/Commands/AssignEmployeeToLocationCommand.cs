using System;
using Domain.Base.Commands;

namespace Domain.Location.Commands
{
    public class AssignEmployeeToLocationCommand : BaseCommand
    {
        public readonly Guid EmployeeId;

        public AssignEmployeeToLocationCommand(Guid locationId, Guid employeeId)
        {
            Id = locationId;
            EmployeeId = employeeId;
        }
    }
}