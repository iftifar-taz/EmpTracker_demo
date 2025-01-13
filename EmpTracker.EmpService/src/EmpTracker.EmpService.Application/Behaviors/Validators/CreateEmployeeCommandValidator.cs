using EmpTracker.EmpService.Application.Features.Employees.Commands;
using FluentValidation;

namespace EmpTracker.EmpService.Application.Behaviors.Validators
{
    public class CreateEmployeeCommandValidator : AbstractValidator<CreateEmployeeCommand>
    {
        public CreateEmployeeCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required.")
                .EmailAddress()
                .WithMessage("Invalid email format.")
                .MaximumLength(64)
                .WithMessage("Email must not exceed 64 characters.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .WithMessage("Phone Number is required.")
                .MaximumLength(64)
                .WithMessage("Phone Number must not exceed 64 characters.");

            RuleFor(x => x.FirstName)
                .MaximumLength(64)
                .WithMessage("First name cannot exceed 64 characters.")
                .When(x => !string.IsNullOrEmpty(x.FirstName));

            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage("Last name is required.")
                .MaximumLength(64).WithMessage("Last name cannot exceed 64 characters.");

            RuleFor(x => x.DateOfBirth)
                .NotEmpty()
                .WithMessage("Date of birth is required.")
                .LessThan(DateTime.Now)
                .WithMessage("Date of birth must be in the past.");

            RuleFor(x => x.DateOfJoining)
                .NotEmpty()
                .WithMessage("Date of joining is required.")
                .GreaterThan(x => x.DateOfBirth)
                .WithMessage("Date of joining must be after the date of birth.");

            RuleFor(x => x.Address)
                .NotEmpty()
                .WithMessage("Address is required.")
                .MaximumLength(256)
                .WithMessage("Address cannot exceed 256 characters.");

            RuleFor(x => x.City)
                .NotEmpty()
                .WithMessage("City is required.")
                .MaximumLength(32)
                .WithMessage("City cannot exceed 32 characters.");

            RuleFor(x => x.State)
                .NotEmpty()
                .WithMessage("State is required.")
                .MaximumLength(32)
                .WithMessage("State cannot exceed 32 characters.");

            RuleFor(x => x.Country)
                .NotEmpty()
                .WithMessage("Country is required.")
                .MaximumLength(32)
                .WithMessage("Country cannot exceed 32 characters.");

            RuleFor(x => x.PostalCode)
                .NotEmpty()
                .WithMessage("Postal code is required.")
                .MaximumLength(16)
                .WithMessage("Postal code cannot exceed 16 characters.");

            RuleFor(x => x.DepartmentId)
                .NotEmpty()
                .WithMessage("Department ID is required.")
                .NotEqual(Guid.Empty)
                .WithMessage("Invalid Department ID.");

            RuleFor(x => x.DesignationId)
                .NotEmpty()
                .WithMessage("Designation ID is required.")
                .NotEqual(Guid.Empty)
                .WithMessage("Invalid Designation ID.");
        }
    }
}
