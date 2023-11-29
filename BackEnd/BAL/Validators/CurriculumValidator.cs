using BAL.Models;
using FluentValidation;
namespace BAL.Validators
{
    public class CurriculumValidator : AbstractValidator<CurriculumViewModel>
    {


        public CurriculumValidator()
        {

            //idSyllabus
            RuleFor(s => s.idSyllabus)
               .Cascade(CascadeMode.StopOnFirstFailure)
               .NotNull().WithMessage("Please enter a {PropertyName} for TrainingProgram")
               .NotEmpty().WithMessage("TrainingProgram {PropertyName} must not be blank");

            //numberOrder
            RuleFor(s => s.numberOrder)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().WithMessage("Please enter a {PropertyName} for TrainingProgram")
                .NotEmpty().WithMessage("TrainingProgram {PropertyName} must not be blank");

        }
    }
}
