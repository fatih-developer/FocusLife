using System;
using MediatR;
using FocusLifePlus.Domain.Common.Enums;

namespace FocusLifePlus.Application.Features.Tasks.Commands.UpdateTask
{
    public class UpdateTaskCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public FocusTaskPriority Priority { get; set; }
        public Guid? CategoryId { get; set; }
        public TimeSpan? EstimatedDuration { get; set; }
        public int? ProgressPercentage { get; set; }
        public FocusTaskStatus? Status { get; set; }
    }
} 