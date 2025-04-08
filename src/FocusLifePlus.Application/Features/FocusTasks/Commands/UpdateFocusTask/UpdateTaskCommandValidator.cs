using System;
using FluentValidation;
using FocusLifePlus.Application.Features.FocusTasks.Commands.UpdateFocusTask;

namespace FocusLifePlus.Application.Features.FocusTasks.Commands.UpdateFocusTask
{
    public class UpdateFocusTaskCommandValidator : AbstractValidator<UpdateFocusTaskCommand>
    {
        public UpdateFocusTaskCommandValidator()
        {
            RuleFor(v => v.Id)
                .NotEmpty().WithMessage("Id is required.");

            RuleFor(v => v.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");

            RuleFor(v => v.Description)
                .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters.");

            RuleFor(v => v.DueDate)
                .NotEmpty().WithMessage("Due date is required.")
                .GreaterThan(DateTime.UtcNow).WithMessage("Due date must be in the future.");

            RuleFor(v => v.ProgressPercentage)
                .InclusiveBetween(0, 100).WithMessage("Progress percentage must be between 0 and 100.")
                .When(v => v.ProgressPercentage.HasValue);
        }
    }
} 