﻿using MediatR;

namespace EmpTracker.DgiService.Application.Features.Designations.Commands
{
    public class UpdateDesignationEmployeeCountCommand(Guid designationId, int amount) : IRequest
    {
        public Guid DesignationId { get; private set; } = designationId;
        public int Amount { get; private set; } = amount;
    }
}
