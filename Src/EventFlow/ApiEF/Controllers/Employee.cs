using System;
using System.Threading;
using System.Threading.Tasks;
using DomainEF.Model.EmployeeModel;
using EventFlow;
using Microsoft.AspNetCore.Mvc;

namespace ApiEF.Controllers
{
    public class Employee : Controller
    {
        private readonly ICommandBus _commandBus;

        public Employee(ICommandBus commandBus)
        {
            _commandBus = commandBus;
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