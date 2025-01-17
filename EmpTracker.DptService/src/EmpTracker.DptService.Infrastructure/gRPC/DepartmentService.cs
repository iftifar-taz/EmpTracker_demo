using Grpc.Core;
using EmpTracker.DptService.Core.Protos;
using MediatR;
using EmpTracker.DptService.Application.Features.Departments.Queries;
using EmpTracker.DptService.Application.Exceptions;

namespace EmpTracker.DptService.Infrastructure.gRPC
{
    public class DepartmentService(IMediator mediator) : DepartmentGrpc.DepartmentGrpcBase
    {
        private readonly IMediator _mediator = mediator;

        public override async Task<CheckIfDepartmentExistsResponse> CheckIfDepartmentExists(CheckIfDepartmentExistsRequest request, ServerCallContext context)
        {
            bool exists;
            try
            {
                var response = await _mediator.Send(new GetDepartmentQuery(Guid.Parse(request.DepartmentId)));
                exists = true;
            }
            catch (NotFoundException)
            {
                exists = false;
            }
            return new CheckIfDepartmentExistsResponse()
            {
                Exists = exists
            };
        }
    }
}
