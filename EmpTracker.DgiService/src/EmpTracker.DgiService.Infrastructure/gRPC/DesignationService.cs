using EmpTracker.DgiService.Application.Exceptions;
using EmpTracker.DgiService.Application.Features.Designations.Queries;
using EmpTracker.DgiService.Core.Protos;
using Grpc.Core;
using MediatR;

namespace EmpTracker.DgiService.Infrastructure.gRPC
{
    public class DesignationService(IMediator mediator) : DesignationGrpc.DesignationGrpcBase
    {
        private readonly IMediator _mediator = mediator;

        public override async Task<CheckIfDesignationExistsResponse> CheckIfDesignationExists(CheckIfDesignationExistsRequest request, ServerCallContext context)
        {
            bool exists;
            try
            {
                var response = await _mediator.Send(new GetDesignationQuery(Guid.Parse(request.DesignationId)));
                exists = true;
            }
            catch (NotFoundException)
            {
                exists = false;
            }
            return new CheckIfDesignationExistsResponse()
            {
                Exists = exists
            };
        }
    }
}
