using System;
using MediatR;
using FocusLifePlus.Domain.Common.Enums;

namespace FocusLifePlus.Application.Features.FocusTasks.Commands.UpdateFocusTask
{
    public class UpdateFocusTaskCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public FocusTaskPriority Priority { get; set; }
        public Guid? CategoryId { get; set; }
        public int? ProgressPercentage { get; set; }
        public TimeSpan? EstimatedDuration { get; set; }
        public TimeSpan? ActualDuration { get; set; }
    }
} 