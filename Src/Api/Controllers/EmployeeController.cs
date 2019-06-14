using System;
using System.Threading;
using System.Threading.Tasks;
using Api.DTOs;
using CQRSlite.Commands;
using CQRSlite.Queries;
using Domain.Base;
using Domain.Employee.Commands;
using Domain.Employee.Queries;
using Domain.Location.Commands;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly ICommandSender _commandSender;
        private readonly IMapper _mapper;
        private readonly IQueryProcessor _queryProcessor;

        public EmployeeController(ICommandSender commandSender, IMapper mapper, IQueryProcessor queryProcessor)
        {
            _commandSender = commandSender;
            _mapper = mapper;
            _queryProcessor = queryProcessor;
        }
        // GET
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken ct = default)
        {
            var employee = await _queryProcessor.Query(new GetEmployeeById(id), ct);
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
        public async Task<IActionResult> GetAll(CancellationToken ct = default)
        {
            var employees = await _queryProcessor.Query(new GetAllEmployees(), ct);
            return Ok(employees);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeRequest request, CancellationToken ct = default)
        {
            var command = await _mapper.Map<CreateEmployeeCommand>(request);
            await _commandSender.Send(command, ct);
            var assignCommand = new AssignEmployeeToLocationCommand(request.LocationId, command.Id);
            await _commandSender.Send(assignCommand, ct);
            return Ok();
        }
    }
}