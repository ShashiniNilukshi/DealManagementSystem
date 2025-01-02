using FluentValidation;
using DealManagementSystem.DTOs;

namespace DealManagementSystem.Validators
{
    public class RegisterUserDTOValidator : AbstractValidator<RegisterUserDTO>
    {
        public RegisterUserDTOValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Please enter a valid email address");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required")
                .Matches("^[a-zA-Z]+$").WithMessage("First name should contain only letters");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required")
                .Matches("^[a-zA-Z]+$").WithMessage("Last name should contain only letters");

            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("Role is required")
                .Must(role => role == "User" || role == "Admin" || role == "SuperAdmin")
                .WithMessage("Role must be one of the following: User, Admin, SuperAdmin");
        }
    }
}
