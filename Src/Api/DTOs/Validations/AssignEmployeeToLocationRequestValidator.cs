using CQRSlite.Queries;
using Domain.Employee;
using Domain.Employee.Queries;
using Domain.Location;
using Domain.Location.Queries;
using FluentValidation;

namespace Api.DTOs.Validations
{
    public class AssignEmployeeToLocationRequestValidator : AbstractValidator<AssignEmployeeToLocationRequest>
    {
        public AssignEmployeeToLocationRequestValidator(IQueryProcessor queryProcessor)
        {
            RuleFor(x => x.LocationId).MustAsync((id, ct) => queryProcessor.Query(new LocationExists(id), ct))
                .WithMessage("No Location with this ID exists.");
            RuleFor(x => x.EmployeeId).MustAsync((id, ct) => queryProcessor.Query(new EmployeeExists(id), ct))
                .WithMessage("No Employee with this ID exists.");
            RuleFor(x => new { x.LocationId, x.EmployeeId })
                .MustAsync(async (x, ct) => !await  queryProcessor.Query(new IsEmployeeAlreadyAssignedToLocation(x.EmployeeId, x.LocationId), ct))
                .WithMessage("This Employee is already assigned to that Location.");
        }
    }
}