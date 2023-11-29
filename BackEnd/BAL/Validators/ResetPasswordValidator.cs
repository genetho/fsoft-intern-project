using BAL.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Validators
{
    public class ResetPasswordValidator : AbstractValidator<PasswordViewModel>
    {
        public ResetPasswordValidator()
        {
            RuleFor(r => r.Otp)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull().WithMessage("{PropertyName} is required")
                .Length(6).WithMessage("Invalid {PropertyName}")
                ;

            RuleFor(r => r.NewPassword)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull().WithMessage("{PropertyName} is required")
                .Length(6, 24).WithMessage("{PropertyName} must be between {MinLength}..{MaxLength} characters");

            RuleFor(r => r.ComrfirmPassword)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .Matches(r => r.NewPassword).WithMessage("New Password and Confirm Password do not match")
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull().WithMessage("{PropertyName} is required")
                .Length(6, 24).WithMessage("{PropertyName} must be between {MinLength}..{MaxLength} characters");
        }
    }
}
