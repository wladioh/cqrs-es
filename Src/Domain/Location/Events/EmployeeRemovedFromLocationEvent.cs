using System;
using Domain.Base.Events;

namespace Domain.Location.Events
{
    public class EmployeeRemovedFromLocationEvent : BaseEvent
    {
        public readonly Guid EmployeeId;

        public EmployeeRemovedFromLocationEvent(Guid id, Guid employeeId)
        {
            Id = id;
            EmployeeId = employeeId;
        }
    }
}