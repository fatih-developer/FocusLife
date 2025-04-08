using FluentValidation;
using FocusLifePlus.Application.Features.FocusTasks.Commands.DeleteFocusTask;

namespace FocusLifePlus.Application.Features.FocusTasks.Commands.DeleteFocusTask
{
    public class DeleteFocusTaskCommandValidator : AbstractValidator<DeleteFocusTaskCommand>
    {
        public DeleteFocusTaskCommandValidator()
        {
            RuleFor(v => v.Id)
                .NotEmpty().WithMessage("Id is required.");
        }
    }
} 