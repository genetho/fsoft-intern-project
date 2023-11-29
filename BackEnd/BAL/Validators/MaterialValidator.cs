using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using BAL.Models;

namespace BAL.Validators
{
    public class MaterialValidatorForEdit : AbstractValidator<MaterialViewModel>
    {
        public MaterialValidatorForEdit()
        {
            //Name
            RuleFor(s => s.Name)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().WithMessage("Lesson material {CollectionIndex} {PropertyName} cannot be blank")
                .NotEmpty().WithMessage("Lesson material {CollectionIndex} {PropertyName} must not be blank")
                .Length(2,50).WithMessage("Lesson material {CollectionIndex} {PropertyName} must has {MinLength}..{MaxLength} characters");
            //HyperLink
            RuleFor(s => s.HyperLink)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().WithMessage("Lesson material {CollectionIndex} {PropertyName} cannot be blank")
                .NotEmpty().WithMessage("Lesson material {CollectionIndex} {PropertyName} cannot be blank")
                .Length(10,500).WithMessage("Lesson material {CollectionIndex} {PropertyName} must have {MinLength}..{MaxLength} characters");
            //Status
            RuleFor(s => s.Status)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().WithMessage("Please enter a {PropertyName} for Lesson material {CollectionIndex}")
                .InclusiveBetween(0, 2).WithMessage("Lesson material {CollectionIndex} {PropertyName} must be between 0..2");
        }
    }
    public class MaterialValidatorForCreate : AbstractValidator<MaterialViewModel>
    {
        public MaterialValidatorForCreate()
        {
            //Id
            RuleFor(m => m.Id).Null().WithName("Material {PropertyName} must be null to create");
            //Name
            RuleFor(s => s.Name)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().WithMessage("Lesson material {CollectionIndex} {PropertyName} cannot be blank")
                .NotEmpty().WithMessage("Lesson material {CollectionIndex} {PropertyName} must not be blank")
                .Length(2, 50).WithMessage("Lesson material {CollectionIndex} {PropertyName} must has {MinLength}..{MaxLength} characters");
            //Hyperlink
            RuleFor(s => s.HyperLink)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().WithMessage("Lesson material {CollectionIndex} {PropertyName} cannot be blank")
                .NotEmpty().WithMessage("Lesson material {CollectionIndex} {PropertyName} cannot be blank")
                .Length(10, 500).WithMessage("Lesson material {CollectionIndex} {PropertyName} must have {MinLength}..{MaxLength} characters");
        }
    }
}
