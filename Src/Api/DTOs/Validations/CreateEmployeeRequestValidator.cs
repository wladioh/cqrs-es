using System;
using CQRSlite.Queries;
using Domain.Employee;
using Domain.Location;
using Domain.Location.Queries;
using FluentValidation;

namespace Api.DTOs.Validations
{
    public class CreateEmployeeRequestValidator : AbstractValidator<CreateEmployeeRequest>
    {
        public CreateEmployeeRequestValidator( IQueryProcessor queryProcessor)
        {
            RuleFor(x => x.LocationId).MustAsync((id, ct)=> queryProcessor.Query(new LocationExists(id), ct))
                .WithMessage("No Location with this ID exists.");
            RuleFor(x => x.FirstName).NotNull().NotEmpty().WithMessage("The First Name cannot be blank.");
            RuleFor(x => x.LastName).NotNull().NotEmpty().WithMessage("The Last Name cannot be blank.");
            RuleFor(x => x.JobTitle).NotNull().NotEmpty().WithMessage("The Job Title cannot be blank.");
            RuleFor(x => x.DateOfBirth).LessThan(DateTime.Today.AddYears(-16)).WithMessage("Employees must be 16 years old or older.");
        }
    }
}