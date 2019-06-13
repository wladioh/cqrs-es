using System;
using System.Threading;
using System.Threading.Tasks;
using CQRSlite.Commands;
using Domain.Base;
using Domain.Employee;
using Domain.Employee.Commands;
using Domain.Location;
using Domain.Location.Commands;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ICommandSender _commandSender;
        private readonly IMapper _mapper;
        private readonly ILocationRepository _locationRepository;

        public EmployeeController(IEmployeeRepository employeeRepository, ICommandSender commandSender, IMapper mapper
            , ILocationRepository locationRepository)
        {
            _employeeRepository = employeeRepository;
            _commandSender = commandSender;
            _mapper = mapper;
            _locationRepository = locationRepository;
        }
        // GET
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var employee = await _employeeRepository.GetById(id);

            //It is possible for GetByID() to return null.
            //If it does, we return HTTP 400 Bad Request
            if (employee == null)
            {
                return BadRequest("No Employee with ID " + id + " was found.");
            }

            //Otherwise, we return the employee
            return Ok(employee);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var employees = await _employeeRepository.GetAll();
            return Ok(employees);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeRequest request, CancellationToken ct)
        {
            var command = await _mapper.Map<CreateEmployeeCommand>(request);
            await _commandSender.Send(command, ct);
            var locationAggregateId = (await _locationRepository.GetById(request.LocationId, ct)).AggregateId;
            var assignCommand = new AssignEmployeeToLocationCommand(locationAggregateId, request.LocationId, request.EmployeeId);
            await _commandSender.Send(assignCommand, ct);
            return Ok();
        }
    }

    public class CreateEmployeeRequest
    {
        public int LocationId { get; set; }
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string JobTitle { get; set; }
    }
    public class CreateEmployeeRequestValidator : AbstractValidator<CreateEmployeeRequest>
    {
        public CreateEmployeeRequestValidator(IEmployeeRepository employeeRepo, ILocationRepository locationRepository)
        {
            RuleFor(x => x.EmployeeId).MustAsync(async (id, ct) => !await employeeRepo.Exists(id, ct))
                .WithMessage("An Employee with this ID already exists.");
            RuleFor(x => x.LocationId).MustAsync(locationRepository.Exists)
                .WithMessage("No Location with this ID exists.");
            RuleFor(x => x.FirstName).NotNull().NotEmpty().WithMessage("The First Name cannot be blank.");
            RuleFor(x => x.LastName).NotNull().NotEmpty().WithMessage("The Last Name cannot be blank.");
            RuleFor(x => x.JobTitle).NotNull().NotEmpty().WithMessage("The Job Title cannot be blank.");
            RuleFor(x => x.DateOfBirth).LessThan(DateTime.Today.AddYears(-16)).WithMessage("Employees must be 16 years old or older.");
        }
    }

}