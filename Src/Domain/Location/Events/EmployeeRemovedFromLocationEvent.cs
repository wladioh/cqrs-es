using System;
using Domain.Base.Events;

namespace Domain.Location.Events
{
    public class EmployeeRemovedFromLocationEvent : BaseEvent
    {
        public readonly int OldLocationId;
        public readonly int EmployeeId;

        public EmployeeRemovedFromLocationEvent(Guid id, int oldLocationId, int employeeId)
        {
            Id = id;
            OldLocationId = oldLocationId;
            EmployeeId = employeeId;
        }
    }
}