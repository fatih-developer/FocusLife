using System;
using MediatR;
using FocusLifePlus.Domain.Common.Enums;

namespace FocusLifePlus.Application.Features.FocusTasks.Commands.CreateFocusTask
{
    public class CreateFocusTaskCommand : IRequest<Guid>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public FocusTaskPriority Priority { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid UserId { get; set; }
        public TimeSpan? EstimatedDuration { get; set; }
    }
} 