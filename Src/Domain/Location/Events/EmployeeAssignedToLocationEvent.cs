using System;
using Domain.Base.Events;

namespace Domain.Location.Events
{
    public class EmployeeAssignedToLocationEvent : BaseEvent
    {
        public readonly int NewLocationId;
        public readonly int EmployeeId;

        public EmployeeAssignedToLocationEvent(Guid id, int newLocationId, int employeeId)
        {
            Id = id;
            NewLocationId = newLocationId;
            EmployeeId = employeeId;
        }
    }
}