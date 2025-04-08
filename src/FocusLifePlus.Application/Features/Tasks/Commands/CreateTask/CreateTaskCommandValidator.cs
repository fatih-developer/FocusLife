using FluentValidation;
using System;

namespace FocusLifePlus.Application.Features.Tasks.Commands.CreateTask
{
    public class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommand>
    {
        public CreateTaskCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Başlık boş olamaz")
                .MaximumLength(200).WithMessage("Başlık 200 karakterden uzun olamaz");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Açıklama 1000 karakterden uzun olamaz");

            RuleFor(x => x.DueDate)
                .Must(dueDate => dueDate > DateTime.UtcNow)
                .WithMessage("Bitiş tarihi gelecekte bir tarih olmalıdır");

            RuleFor(x => x.Priority)
                .IsInEnum().WithMessage("Geçersiz öncelik değeri");

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("Kullanıcı ID boş olamaz");

            RuleFor(x => x.EstimatedDuration)
                .Must(duration => !duration.HasValue || duration.Value.TotalMinutes > 0)
                .WithMessage("Tahmini süre pozitif bir değer olmalıdır");
        }
    }
} 