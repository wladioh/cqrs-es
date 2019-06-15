using System;
using System.Threading;
using System.Threading.Tasks;
using DomainEF.Model.EmployeeModel;
using EventFlow;
using EventFlow.Queries;
using Microsoft.AspNetCore.Mvc;

namespace ApiEF.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly ICommandBus _commandBus;
        private readonly IQueryProcessor _queryExecutor;

        public EmployeeController(ICommandBus commandBus, IQueryProcessor queryExecutor)
        {
            _commandBus = commandBus;
            _queryExecutor = queryExecutor;
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id, CancellationToken ct)
        {
            var x = await _queryExecutor.ProcessAsync(new ReadModelByIdQuery<EmployeeReadModel>(EmployeeId.With(id)), ct);
            return Ok(x);
        }
        
        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken ct)
        {
            var x = await _queryExecutor.ProcessAsync(new GetAllEmployee(), ct);
            return Ok(x);
        }
        [HttpPost]
        public async Task<IActionResult> Post(CreateEmployeeRequest request, CancellationToken ct)
        {
            var cargoId = EmployeeId.New;
            await _commandBus.PublishAsync(new CreateEmployeeCommand(cargoId, request.FirstName, request.LastName, request.DateOfBirth, request.JobTitle), ct)
                .ConfigureAwait(false);
            return Ok(cargoId.Value);
        }
    }

    public class CreateEmployeeRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string JobTitle { get; set; }
    }
}