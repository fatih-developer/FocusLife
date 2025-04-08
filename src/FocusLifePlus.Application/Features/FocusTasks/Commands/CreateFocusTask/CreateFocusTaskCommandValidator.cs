using System;
using FluentValidation;
using FocusLifePlus.Application.Common.Exceptions;
using FocusLifePlus.Application.Features.FocusTasks.Commands.CreateFocusTask;

namespace FocusLifePlus.Application.Features.FocusTasks.Commands.CreateFocusTask
{
    public class CreateFocusTaskCommandValidator : AbstractValidator<CreateFocusTaskCommand>
    {
        public CreateFocusTaskCommandValidator()
        {
            RuleFor(v => v.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");

            RuleFor(v => v.Description)
                .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters.");

            RuleFor(v => v.DueDate)
                .NotEmpty().WithMessage("Due date is required.")
                .GreaterThan(DateTime.UtcNow).WithMessage("Due date must be in the future.");

            RuleFor(v => v.UserId)
                .NotEmpty().WithMessage("User ID is required.");
        }
    }
} 