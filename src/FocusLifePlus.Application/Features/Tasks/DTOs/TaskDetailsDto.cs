using System;
using System.Collections.Generic;
using FocusLifePlus.Domain.Common.Enums;

namespace FocusLifePlus.Application.Features.Tasks.DTOs
{
    public class TaskDetailsDto
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
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public int ProgressPercentage { get; set; }
        public TimeSpan? EstimatedDuration { get; set; }
        public TimeSpan? ActualDuration { get; set; }
        public bool IsOverdue { get; set; }
        public bool IsCompleted { get; set; }
        public List<SubTaskDto> SubTasks { get; set; }
        public List<string> Tags { get; set; }
        public List<ReminderDto> Reminders { get; set; }
    }

    public class SubTaskDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public bool IsCompleted { get; set; }
    }

    public class ReminderDto
    {
        public Guid Id { get; set; }
        public DateTime ReminderTime { get; set; }
        public string Note { get; set; }
    }
} 