using System;
using FocusLifePlus.Domain.Common.Enums;

namespace FocusLifePlus.Application.Features.Tasks.DTOs
{
    public class TaskListDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime DueDate { get; set; }
        public FocusTaskPriority Priority { get; set; }
        public FocusTaskStatus Status { get; set; }
        public string CategoryName { get; set; }
        public int ProgressPercentage { get; set; }
        public bool IsOverdue { get; set; }
        public bool IsCompleted { get; set; }
        public int SubTaskCount { get; set; }
        public int CompletedSubTaskCount { get; set; }
    }
} 