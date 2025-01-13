using EmpTracker.DptService.Application.Features.Departments.Commands;
using FluentValidation;

namespace EmpTracker.DptService.Application.Behaviors.Validators
{
    public class UpdateDepartmentCommandValidator : AbstractValidator<UpdateDepartmentCommand>
    {
        public UpdateDepartmentCommandValidator()
        {
            RuleFor(x => x.DepartmentName)
                .NotEmpty()
                .WithMessage("Department Name is required.")
                .MaximumLength(64)
                .WithMessage("Department Name must not exceed 64 characters.");

            RuleFor(x => x.DepartmentKey)
                .NotEmpty()
                .WithMessage("Department Key is required.")
                .MaximumLength(16)
                .WithMessage("Department Key must not exceed 16 characters.");
        }
    }
}
