using System;
using BAL.Models;
using System.Linq;
using System.Text;
using FluentValidation;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BAL.Validators
{
    public class TrainingProgramValidator : AbstractValidator<ProgramViewModel>
    {


        public TrainingProgramValidator()
        {

            //Name
            RuleFor(s => s.name)
               .Cascade(CascadeMode.StopOnFirstFailure)
               .NotNull().WithMessage("Please enter a {PropertyName} for TrainingProgram");
            //Syllabus
            RuleFor(s => s.syllabi)
               .Cascade(CascadeMode.StopOnFirstFailure)
               .NotNull().WithMessage("Please enter a {PropertyName} for TrainingProgram");
            //id
            RuleForEach(TrainingProgram => TrainingProgram.syllabi)
               .SetValidator(new CurriculumValidator());
        }
    }

    public class TrainingProgramValidatorForDelete : AbstractValidator<TrainingProgramViewModel>
    {

        public TrainingProgramValidatorForDelete()
        {
            //ID
            RuleFor(s => s.Id)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Please enter an Id for Training Program")
                .NotNull().WithMessage("Please enter an Id for Training Program");

            RuleFor(s => s.Id).GreaterThan(0).WithMessage("The Training id must be larger than 0");
        }
    }
    public class TrainingProgramValidatorForDuplicate : AbstractValidator<TrainingProgramViewModel>
    {
        public TrainingProgramValidatorForDuplicate()
    {
            //ID
            RuleFor(s => s.Id)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Please enter an Id for Training Program")
                .NotNull().WithMessage("Please enter an Id for Training Program");

            RuleFor(s => s.Id).GreaterThan(0).WithMessage("The Training id must be larger than 0");
        }
    }
    public class TrainingProgramValidatorForDeActive : AbstractValidator<TrainingProgramViewModel>
    {
        public TrainingProgramValidatorForDeActive()
        {
            //ID
            RuleFor(s => s.Id)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Please enter an Id for Training Program")
                .NotNull().WithMessage("Please enter an Id for Training Program");

            RuleFor(s => s.Id).GreaterThan(0).WithMessage("The Training id must be larger than 0");
        }
    }
    public class TrainingProgramValidatorForEdit : AbstractValidator<TrainingProgramViewModel>
        {
        public TrainingProgramValidatorForEdit()
        {
            //ID
            RuleFor(s => s.Id)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Please enter an Id for Training Program")
                .NotNull().WithMessage("Please enter an Id for Training Program");

            RuleFor(s => s.Id).GreaterThan(0).WithMessage("The Training id must be larger than 0");
            //Name
            RuleFor(s => s.Name)
               .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Training Program {PropertyName} must not be blank")
                    .When(s => s.Name.Trim().Equals(""), ApplyConditionTo.CurrentValidator)
                .NotNull().WithMessage("Please enter a {PropertyName} for Training Program")
                .Length(2, 500).WithMessage("{PropertyName} must be between {MinLength}..{MaxLength} characters");
            //Status
            RuleFor(s => s.Status)
               .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Please enter an Status ID for Training Program")
                .NotNull().WithMessage("Please enter an Status ID for Training Program");

            RuleFor(s => s.Status).GreaterThan(-1).WithMessage("The Status ID must be equal or more than 0");
            RuleFor(s => s.Status).LessThan(9).WithMessage("The Status ID must be less than 9");
        }
    }
}
