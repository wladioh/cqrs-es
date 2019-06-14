using System;
using Domain.Base.Events;

namespace Domain.Location.Events
{
    public class EmployeeAssignedToLocationEvent : BaseEvent
    {
        public readonly Guid EmployeeId;

        public EmployeeAssignedToLocationEvent(Guid id, Guid employeeId)
        {
            Id = id;
            EmployeeId = employeeId;
        }
    }
}