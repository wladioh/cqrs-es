using System;
using System.Threading;
using System.Threading.Tasks;
using DomainEF.Model.Location;
using EventFlow;
using EventFlow.Queries;
using Microsoft.AspNetCore.Mvc;

namespace ApiEF.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class LocationController : Controller
    {
        private readonly ICommandBus _commandBus;
        private readonly IQueryProcessor _queryExecutor;

        public LocationController(ICommandBus commandBus, IQueryProcessor queryExecutor)
        {
            _commandBus = commandBus;
            _queryExecutor = queryExecutor;
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Post(Guid id, CancellationToken ct)
        {
            var locationReadModel = await _queryExecutor.ProcessAsync(new ReadModelByIdQuery<LocationReadModel>(LocationId.With(id)), ct);
            return Ok(locationReadModel);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateLocationRequest request, CancellationToken ct)
        {
            var id = LocationId.New;
            await _commandBus.PublishAsync(new CreateLocationCommand(id, request.StreetAddress, request.City, request.State,
                request.PostalCode), ct);
            return Ok(id.GetGuid());
        }
    }

    public class CreateLocationRequest
    {
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
    }
}