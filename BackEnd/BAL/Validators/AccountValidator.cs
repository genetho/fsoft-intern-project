using BAL.Models;
using BAL.Services.Implements;
using BAL.Services.Interfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BAL.Validators
{
    public class AccountValidator : AbstractValidator<AccountViewModel>
    {
        public AccountValidator()
        {
            RuleFor(a => a.Email)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .Matches(@"^(\w+@\w+\.\w+)$").WithMessage("A valid {PropertyName} is required")
                .NotEmpty().WithMessage("{PropertyName} address is required")
                .NotNull().WithMessage("{PropertyName} address is required");
            //.EmailAddress().WithMessage("A valid {PropertyName} is required")

            RuleFor(a => a.Password)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull().WithMessage("{PropertyName} is required");
        }
    }

    public class AccountValidatorForAdd : AbstractValidator<UserAccountViewModel>
    {
        private readonly IRoleService _roleService;
        public AccountValidatorForAdd(IRoleService roleService)
        {
            _roleService = roleService;

            RuleFor(a => a.UserName)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull().WithMessage("{PropertyName} address is required")
                .Length(3, 30).WithMessage("{PropertyName} must be between {MinLength}..{MaxLength} characters");

            RuleFor(a => a.Password)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull().WithMessage("{PropertyName} is required")
                .Length(6, 24).WithMessage("{PropertyName} must be between {MinLength}..{MaxLength} characters");

            RuleFor(a => a.Fullname)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull().WithMessage("{PropertyName} is required")
                .Length(2, 500).WithMessage("{PropertyName} must be between {MinLength}..{MaxLength} characters");

            RuleFor(a => a.DateOfBirth.ToString())
                .Cascade(CascadeMode.StopOnFirstFailure)
                .Must(Utility.BeAValidDate).WithMessage("Invalid Date of Birth")
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull().WithMessage("{PropertyName} is required");

            RuleFor(a => a.Gender)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .Must(Utility.BeAValidGender).WithMessage("Invalid {PropertyName}")
                .NotEmpty().WithMessage("Invalid {PropertyName}")
                .NotNull().WithMessage("Invalid {PropertyName}");

            RuleFor(a => a.Phone)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .Must(Utility.BeAValidPhone).WithMessage("Invalid phone number");

            RuleFor(a => a.Email)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .Matches(@"^(\w+@\w+\.\w+)$").WithMessage("A valid {PropertyName} is required")
                .NotEmpty().WithMessage("{PropertyName} address is required")
                .NotNull().WithMessage("{PropertyName} address is required");
            //.EmailAddress().WithMessage("A valid {PropertyName} is required")

            RuleFor(r => r.Status)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .Must(Utility.BeAValidStatus).WithMessage("Invalid {PropertyName}")
                .NotEmpty().WithMessage("Invalid {PropertyName}")
                .NotNull().WithMessage("Invalid {PropertyName}");

            RuleFor(r => r.IdRole)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .Must(BeAValidRole).WithMessage("Invalid user's role")
                .NotEmpty().WithMessage("Invalid user's role")
                .NotNull().WithMessage("Invalid user's role");
        }

        private bool BeAValidRole(long value)
        {
            var result = _roleService.GetRoleById(value);
            if (result != null)
                return true;
            return false;
        }

    }
    public class AccountValidatorForEdit : AbstractValidator<UserViewModel>
    {
        private readonly IRoleService _roleService;
        public AccountValidatorForEdit(IRoleService roleService)
        {
            _roleService = roleService;

            RuleFor(a => a.ID.ToString())
                .Cascade(CascadeMode.StopOnFirstFailure)
                .Must(Utility.BeAValidNumber).WithMessage("{PropertyName} must be a number")
                .NotEmpty().WithMessage("Please enter an Id for user")
                .NotNull().WithMessage("Please enter an Id for user");

            RuleFor(a => a.Fullname)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull().WithMessage("{PropertyName} is required")
                .Length(2, 500).WithMessage("{PropertyName} must be between {MinLength}..{MaxLength} characters");

            RuleFor(a => a.DateOfBirth.ToString())
                .Cascade(CascadeMode.StopOnFirstFailure)
                .Must(Utility.BeAValidDate).WithMessage("Invalid Date of Birth")
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull().WithMessage("{PropertyName} is required");

            RuleFor(a => a.Gender)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .Must(Utility.BeAValidGender).WithMessage("Invalid {PropertyName}")
                .NotEmpty().WithMessage("Invalid {PropertyName}")
                .NotNull().WithMessage("Invalid {PropertyName}");

            RuleFor(a => a.Phone)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .Must(Utility.BeAValidPhone).WithMessage("Invalid phone number");

            RuleFor(a => a.Email)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .Matches(@"^(\w+@\w+\.\w+)$").WithMessage("A valid {PropertyName} is required")
                .NotEmpty().WithMessage("{PropertyName} address is required")
                .NotNull().WithMessage("{PropertyName} address is required");
            //.EmailAddress().WithMessage("A valid {PropertyName} is required")

            RuleFor(r => r.Status)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .Must(Utility.BeAValidStatus).WithMessage("Invalid {PropertyName}")
                .NotEmpty().WithMessage("Invalid {PropertyName}")
                .NotNull().WithMessage("Invalid {PropertyName}");

            RuleFor(r => r.IdRole)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .Must(BeAValidRole).WithMessage("Invalid user's role")
                .NotEmpty().WithMessage("Invalid user's role")
                .NotNull().WithMessage("Invalid user's role");
        }
        private bool BeAValidRole(long value)
        {
            var result = _roleService.GetRoleById(value);
            if (result != null)
                return true;
            return false;
        }
    }

    public static class Utility
    {
        public static bool BeAValidGender(char gender)
        {
            if (gender == 'M' || gender == 'F')
                return true;
            return false;
        }

        public static bool BeAValidDate(string value)
        {
            return DateTime.TryParse(value, out DateTime date);
        }

        public static bool BeAValidPhone(string value)
        {
            return Regex.IsMatch(value, @"^(0\d{9,10})$");
        }

        public static bool BeAValidStatus(int value)
        {
            if (value == 1)
                return true;
            return false;
        }

        public static bool BeAValidNumber(string value)
        {
            return long.TryParse(value, out long result);
        }
    }
}
