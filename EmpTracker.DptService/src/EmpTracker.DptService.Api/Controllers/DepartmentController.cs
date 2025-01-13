using Asp.Versioning;
using EmpTracker.DptService.Application.Dtos;
using EmpTracker.DptService.Application.Features.Departments.Commands;
using EmpTracker.DptService.Application.Features.Departments.Queries;
using EmpTracker.DptService.Core.Domain.Attribures;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EmpTracker.DptService.Api.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/departments")]
    public class DepartmentController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [PermissionRequirement("view.department")]
        [HttpGet("", Name = "GetDepartments")]
        public async Task<ActionResult<IEnumerable<DepartmentResponseDto>>> GetDepartments()
        {
            var response = await _mediator.Send(new GetDepartmentsQuery());
            return Ok(response);
        }

        [PermissionRequirement("view.department")]
        [HttpGet("{departmentId}", Name = "GetDepartment")]
        public async Task<ActionResult<DepartmentResponseDto>> GetDepartment([FromRoute] Guid departmentId)
        {
            var response = await _mediator.Send(new GetDepartmentQuery(departmentId));
            return Ok(response);
        }

        [PermissionRequirement("create.department")]
        [HttpPost("", Name = "CreateDepartment")]
        public async Task<IActionResult> CreateDepartment([FromBody] CreateDepartmentCommand command)
        {
            await _mediator.Send(command);
            return Created();
        }

        [PermissionRequirement("update.department")]
        [HttpPut("{departmentId}", Name = "UpdateDepartment")]
        public async Task<IActionResult> UpdateDepartment([FromRoute] Guid departmentId, [FromBody] UpdateDepartmentCommand command)
        {
            command.DepartmentId = departmentId;
            await _mediator.Send(command);
            return Ok();
        }

        [PermissionRequirement("delete.department")]
        [HttpDelete("{departmentId}", Name = "DeleteDepartment")]
        public async Task<IActionResult> DeleteDepartment([FromRoute] Guid departmentId)
        {
            await _mediator.Send(new DeleteDepartmentCommand(departmentId));
            return Ok();
        }

        [PermissionRequirement("view.department.employee")]
        [HttpGet("{departmentId}/employees", Name = "GetDepartmentEmployees")]
        public async Task<ActionResult<IEnumerable<DepartmentEmployeeResponseDto>>> GetDepartmentEmployees([FromRoute] Guid departmentId)
        {
            var response = await _mediator.Send(new GetDepartmentEmployeesQuery(departmentId));
            return Ok(response);
        }
    }
}
