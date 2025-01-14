using Asp.Versioning;
using EmpTracker.DgiService.Application.Dtos;
using EmpTracker.DgiService.Application.Features.Designations.Commands;
using EmpTracker.DgiService.Application.Features.Designations.Queries;
using EmpTracker.DgiService.Core.Domain.Attribures;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EmpTracker.DgiService.Api.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/Designations")]
    public class DesignationController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [PermissionRequirement("view.designation")]
        [HttpGet("", Name = "GetDesignations")]
        public async Task<ActionResult<IEnumerable<DesignationResponseDto>>> GetDesignations()
        {
            var response = await _mediator.Send(new GetDesignationsQuery());
            return Ok(response);
        }

        [PermissionRequirement("view.designation")]
        [HttpGet("{designationId}", Name = "GetDesignation")]
        public async Task<ActionResult<DesignationResponseDto>> GetDesignation([FromRoute] Guid designationId)
        {
            var response = await _mediator.Send(new GetDesignationQuery(designationId));
            return Ok(response);
        }

        [PermissionRequirement("create.designation")]
        [HttpPost("", Name = "CreateDesignation")]
        public async Task<IActionResult> CreateDesignation([FromBody] CreateDesignationCommand command)
        {
            await _mediator.Send(command);
            return Created();
        }

        [PermissionRequirement("update.designation")]
        [HttpPut("{designationId}", Name = "UpdateDesignation")]
        public async Task<IActionResult> UpdateDesignation([FromRoute] Guid designationId, [FromBody] UpdateDesignationCommand command)
        {
            command.DesignationId = designationId;
            await _mediator.Send(command);
            return Ok();
        }

        [PermissionRequirement("delete.designation")]
        [HttpDelete("{designationId}", Name = "DeleteDesignation")]
        public async Task<IActionResult> DeleteDesignation([FromRoute] Guid designationId)
        {
            await _mediator.Send(new DeleteDesignationCommand(designationId));
            return Ok();
        }

        [PermissionRequirement("view.designation.employee")]
        [HttpGet("{designationId}/employees", Name = "GetDesignationEmployees")]
        public async Task<ActionResult<IEnumerable<DesignationEmployeeResponseDto>>> GetDesignationEmployees([FromRoute] Guid designationId)
        {
            var response = await _mediator.Send(new GetDesignationEmployeesQuery(designationId));
            return Ok(response);
        }
    }
}
