using System;
using FocusLifePlus.Domain.Entities;
using FocusLifePlus.Domain.Common.Enums;

namespace FocusLifePlus.Application.Features.FocusTasks.Queries.GetFocusTaskById
{
    public class FocusTaskDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public FocusTaskPriority Priority { get; set; }
        public FocusTaskStatus Status { get; set; }
        public Guid? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
    }
} 