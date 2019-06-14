using System;
using System.Threading;
using System.Threading.Tasks;
using Api.DTOs;
using CQRSlite.Commands;
using CQRSlite.Queries;
using Domain.Base;
using Domain.Employee.Queries;
using Domain.Location.Commands;
using Domain.Location.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly ICommandSender _commandSender;
        private readonly IMapper _mapper;
        private readonly IQueryProcessor _queryProcessor;

        public LocationController(ICommandSender commandSender, IMapper mapper, IQueryProcessor queryProcessor)
        {
            _commandSender = commandSender;
            _mapper = mapper;
            _queryProcessor = queryProcessor;
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken ct = default)
        {
            var location = await _queryProcessor.Query(new GetLocationById(id), ct);
            if (location == null)
            {
                return BadRequest("No location with ID " + id.ToString() + " was found.");
            }
            return Ok(location);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct = default)
        {
            var locations = await _queryProcessor.Query(new GetAllLocations(), ct);
            return Ok(locations);
        }

        [HttpGet("{id:guid}/employees")]
        public async Task<IActionResult> GetEmployees(Guid id, CancellationToken ct = default)
        {
            var employees = await _queryProcessor.Query(new GetEmployeesFromLocation(id), ct);
            return Ok(employees);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateLocationRequest request, CancellationToken ct = default)
        {
            var command = await _mapper.Map<CreateLocationCommand>(request);
            await _commandSender.Send(command, ct);
            return Ok();
        }

        [HttpPost("assignemployee")]
        public async Task<IActionResult> AssignEmployee(AssignEmployeeToLocationRequest request, CancellationToken ct = default)
        {
            var employee = await _queryProcessor.Query(new GetEmployeeById(request.EmployeeId), ct);
            if (Guid.Empty != employee.LocationId)
            {
                var removeCommand = new RemoveEmployeeFromLocationCommand(employee.LocationId, request.LocationId, employee.Id);
                await _commandSender.Send(removeCommand, ct);
            }

            var assignCommand = new AssignEmployeeToLocationCommand(request.LocationId, request.EmployeeId);
            await _commandSender.Send(assignCommand, ct);

            return Ok();
        }
    }
}