using System.Threading.Tasks;
using CQRSlite.Commands;
using Domain.Base;
using Domain.Employee;
using Domain.Location;
using Domain.Location.Commands;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly ILocationRepository _locationRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ICommandSender _commandSender;
        private readonly IMapper _mapper;

        public LocationController(ILocationRepository locationRepository, 
            IEmployeeRepository employeeRepository,
            ICommandSender commandSender, IMapper mapper)
        {
            _locationRepository = locationRepository;
            _employeeRepository = employeeRepository;
            _commandSender = commandSender;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var location = await _locationRepository.GetById(id);
            if (location == null)
            {
                return BadRequest("No location with ID " + id.ToString() + " was found.");
            }
            return Ok(location);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var locations = await _locationRepository.GetAll();
            return Ok(locations);
        }

        [HttpGet("{id}/employees")]
        public async Task<IActionResult> GetEmployees(int id)
        {
            var employees = await _locationRepository.GetEmployees(id);
            return Ok(employees);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateLocationRequest request)
        {
            var command = await _mapper.Map<CreateLocationCommand>(request);
            await _commandSender.Send(command);
            return Ok();
        }

        [HttpPost("assignemployee")]
        public async Task<IActionResult> AssignEmployee(AssignEmployeeToLocationRequest request)
        {
            var employee = await _employeeRepository.GetById(request.EmployeeId);
            if (employee.LocationId != 0)
            {
                var oldLocationAggregateId = (await _locationRepository.GetById(employee.LocationId)).AggregateId;

                var removeCommand = new RemoveEmployeeFromLocationCommand(oldLocationAggregateId, request.LocationId, employee.EmployeeId);
                await _commandSender.Send(removeCommand);
            }

            var locationAggregateId = (await _locationRepository.GetById(request.LocationId)).AggregateId;
            var assignCommand = new AssignEmployeeToLocationCommand(locationAggregateId, request.LocationId, request.EmployeeId);
            await _commandSender.Send(assignCommand);

            return Ok();
        }
    }

    public class CreateLocationRequest
    {
        public int LocationId { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
    }

    public class CreateLocationRequestValidator : AbstractValidator<CreateLocationRequest>
    {
        public CreateLocationRequestValidator(ILocationRepository locationRepo)
        {
            RuleFor(x => x.LocationId).MustAsync(async (x, ct) => !await locationRepo.Exists(x, ct))
                .WithMessage("A Location with this ID already exists.");
            RuleFor(x => x.StreetAddress).NotNull().NotEmpty().WithMessage("The Street Address cannot be null");
            RuleFor(x => x.City).NotNull().NotEmpty().WithMessage("The City cannot be null");
            RuleFor(x => x.State).NotNull().NotEmpty().WithMessage("The State cannot be null");
            RuleFor(x => x.PostalCode).NotNull().NotEmpty().WithMessage("The Postal Code cannot be null");
        }
    }

    public class AssignEmployeeToLocationRequest
    {
        public int LocationId { get; set; }
        public int EmployeeId { get; set; }
    }

    public class AssignEmployeeToLocationRequestValidator : AbstractValidator<AssignEmployeeToLocationRequest>
    {
        public AssignEmployeeToLocationRequestValidator(IEmployeeRepository employeeRepo, ILocationRepository locationRepo)
        {
            RuleFor(x => x.LocationId).MustAsync(locationRepo.Exists)
                .WithMessage("No Location with this ID exists.");
            RuleFor(x => x.EmployeeId).MustAsync(employeeRepo.Exists)
                .WithMessage("No Employee with this ID exists.");
            RuleFor(x => new { x.LocationId, x.EmployeeId })
                .MustAsync(async (x, ct) => !await locationRepo.HasEmployee(x.LocationId, x.EmployeeId))
                .WithMessage("This Employee is already assigned to that Location.");
        }
    }

}