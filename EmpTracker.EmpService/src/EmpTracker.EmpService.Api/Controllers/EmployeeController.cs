using Asp.Versioning;
using EmpTracker.EmpService.Application.Dtos;
using EmpTracker.EmpService.Application.Features.Employees.Commands;
using EmpTracker.EmpService.Application.Features.Employees.Queries;
using EmpTracker.EmpService.Core.Domain.Attribures;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EmpTracker.EmpService.Api.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/Employees")]
    public class EmployeeController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [PermissionRequirement("view.employee")]
        [HttpGet("", Name = "GetEmployees")]
        public async Task<ActionResult<IEnumerable<EmployeeResponseDto>>> GetEmployees()
        {
            var response = await _mediator.Send(new GetEmployeesQuery());
            return Ok(response);
        }

        [PermissionRequirement("view.employee")]
        [HttpGet("{employeeId}", Name = "GetEmployee")]
        public async Task<ActionResult<EmployeeResponseDto>> GetEmployee([FromRoute] Guid employeeId)
        {
            var response = await _mediator.Send(new GetEmployeeQuery(employeeId));
            return Ok(response);
        }

        [PermissionRequirement("create.employee")]
        [HttpPost("", Name = "CreateEmployee")]
        public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeCommand command)
        {
            await _mediator.Send(command);
            return Created();
        }

        [PermissionRequirement("update.employee")]
        [HttpPut("{employeeId}", Name = "UpdateEmployee")]
        public async Task<IActionResult> UpdateEmployee([FromRoute] Guid employeeId, [FromBody] UpdateEmployeeCommand command)
        {
            command.EmployeeId = employeeId;
            await _mediator.Send(command);
            return Ok();
        }

        [PermissionRequirement("delete.employee")]
        [HttpDelete("{employeeId}", Name = "DeleteEmployee")]
        public async Task<IActionResult> DeleteEmployee([FromRoute] Guid employeeId)
        {
            await _mediator.Send(new DeleteEmployeeCommand(employeeId));
            return Ok();
        }
    }
}
