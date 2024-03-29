﻿using System;
using Domain.Base;

namespace Domain.Employee
{
    public class EmployeeRM: IEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string JobTitle { get; set; }
        public Guid LocationId { get; set; }
        public Guid Id { get; set; }
    }
}