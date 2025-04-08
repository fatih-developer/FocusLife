using FluentValidation;
using System;

namespace FocusLifePlus.Application.Features.Tasks.Commands.UpdateTask
{
    public class UpdateTaskCommandValidator : AbstractValidator<UpdateTaskCommand>
    {
        public UpdateTaskCommandValidator()
        {
            RuleFor(v => v.Id)
                .NotEmpty().WithMessage("Görev ID boş olamaz.");

            RuleFor(v => v.Title)
                .NotEmpty().WithMessage("Başlık boş olamaz.")
                .MaximumLength(200).WithMessage("Başlık 200 karakterden uzun olamaz.");

            RuleFor(v => v.Description)
                .MaximumLength(1000).WithMessage("Açıklama 1000 karakterden uzun olamaz.");

            RuleFor(v => v.DueDate)
                .NotEmpty().WithMessage("Bitiş tarihi boş olamaz.");

            RuleFor(v => v.Priority)
                .IsInEnum().WithMessage("Geçersiz öncelik değeri.");

            RuleFor(v => v.Status)
                .Must(x => !x.HasValue || Enum.IsDefined(typeof(Domain.Common.Enums.FocusTaskStatus), x.Value))
                .WithMessage("Geçersiz durum değeri.");

            RuleFor(v => v.ProgressPercentage)
                .Must(x => !x.HasValue || (x.Value >= 0 && x.Value <= 100))
                .WithMessage("İlerleme yüzdesi 0-100 arasında olmalıdır.");

            RuleFor(v => v.EstimatedDuration)
                .Must(x => !x.HasValue || x.Value.TotalMinutes > 0)
                .WithMessage("Tahmini süre 0'dan büyük olmalıdır.");
        }
    }
} 