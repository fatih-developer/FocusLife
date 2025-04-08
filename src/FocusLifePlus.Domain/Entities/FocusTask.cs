using System;
using System.Collections.Generic;
using FocusLifePlus.Domain.Common;
using FocusLifePlus.Domain.Common.Enums;


namespace FocusLifePlus.Domain.Entities
{
    public class FocusTask : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public FocusTaskPriority Priority { get; set; }
        public FocusTaskStatus Status { get; set; }
        public Guid? FocusCategoryId { get; set; }
        public FocusCategory FocusCategory { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public int ProgressPercentage { get; set; } = 0;
        public TimeSpan? EstimatedDuration { get; set; }
        public TimeSpan? ActualDuration { get; set; }
        
        // Alt görevler
        public ICollection<FocusSubTask> SubTasks { get; set; } = new List<FocusSubTask>();
        
        // Etiketler
        public ICollection<FocusTag> Tags { get; set; } = new List<FocusTag>();
        
        // Hatırlatıcılar
        public ICollection<FocusReminder> Reminders { get; set; } = new List<FocusReminder>();
        
        // Tekrarlanan görevler
        public FocusRecurrenceRule RecurrenceRule { get; set; }
        public bool IsRecurring => RecurrenceRule != null;
        
        // Görev bağımlılıkları
        public ICollection<FocusTaskDependency> Dependencies { get; set; } = new List<FocusTaskDependency>();
        public ICollection<FocusTaskDependency> Dependents { get; set; } = new List<FocusTaskDependency>();
        
        // Görev geçmişi
        public ICollection<FocusTaskHistory> History { get; set; } = new List<FocusTaskHistory>();
        
        // Görev yorumları
        public ICollection<FocusTaskComment> Comments { get; set; } = new List<FocusTaskComment>();
        
        // Görev atamaları
        public ICollection<FocusTaskAssignment> Assignments { get; set; } = new List<FocusTaskAssignment>();

        public bool IsCompleted => Status == FocusTaskStatus.Done;
        public bool IsOverdue => !IsCompleted && DueDate < DateTime.UtcNow;
        public bool IsDueToday => !IsCompleted && DueDate.Date == DateTime.UtcNow.Date;
        public bool IsDueThisWeek => !IsCompleted && DueDate.Date >= DateTime.UtcNow.Date && DueDate.Date <= DateTime.UtcNow.AddDays(7).Date;
        public bool IsDueThisMonth => !IsCompleted && DueDate.Date >= DateTime.UtcNow.Date && DueDate.Date <= DateTime.UtcNow.AddMonths(1).Date;
        public TimeSpan? CompletionTime => CompletedAt.HasValue ? CompletedAt.Value - CreatedAt : null;

        public FocusTask()
        {
            CreatedAt = DateTime.UtcNow;
            Status = FocusTaskStatus.Todo;
            ProgressPercentage = 0;
        }

        public void Complete()
        {
            Status = FocusTaskStatus.Done;
            CompletedAt = DateTime.UtcNow;
            ProgressPercentage = 100;
            
            // Geçmişe kaydet
            AddHistory("Status", "InProgress", "Done");
        }

        public void Update(string title, string description, DateTime dueDate, FocusTaskPriority priority)
        {
            var oldTitle = Title;
            var oldDescription = Description;
            var oldDueDate = DueDate;
            var oldPriority = Priority;
            
            Title = title;
            Description = description;
            DueDate = dueDate;
            Priority = priority;
            
            // Geçmişe kaydet
            if (oldTitle != title) AddHistory("Title", oldTitle, title);
            if (oldDescription != description) AddHistory("Description", oldDescription, description);
            if (oldDueDate != dueDate) AddHistory("DueDate", oldDueDate.ToString(), dueDate.ToString());
            if (oldPriority != priority) AddHistory("Priority", oldPriority.ToString(), priority.ToString());
        }

        public void UpdateProgress(int progressPercentage)
        {
            if (progressPercentage < 0 || progressPercentage > 100)
                throw new ArgumentException("Progress percentage must be between 0 and 100");

            var oldProgress = ProgressPercentage;
            ProgressPercentage = progressPercentage;
            
            // Geçmişe kaydet
            AddHistory("ProgressPercentage", oldProgress.ToString(), progressPercentage.ToString());
            
            if (progressPercentage == 100)
                Complete();
            else if (progressPercentage > 0)
            {
                var oldStatus = Status;
                Status = FocusTaskStatus.InProgress;
                
                // Geçmişe kaydet
                if (oldStatus != Status) AddHistory("Status", oldStatus.ToString(), Status.ToString());
            }
        }
        
        public void AddSubTask(FocusSubTask subTask)
        {
            SubTasks.Add(subTask);
            subTask.ParentTask = this;
            
            // Geçmişe kaydet
            AddHistory("SubTasks", "Added", subTask.Title);
        }
        
        public void AddTag(FocusTag tag)
        {
            Tags.Add(tag);
            
            // Geçmişe kaydet
            AddHistory("Tags", "Added", tag.Name);
        }
        
        public void RemoveTag(FocusTag tag)
        {
            Tags.Remove(tag);
            
            // Geçmişe kaydet
            AddHistory("Tags", "Removed", tag.Name);
        }
        
        public void AddReminder(FocusReminder reminder)
        {
            Reminders.Add(reminder);
            reminder.Task = this;
            
            // Geçmişe kaydet
            AddHistory("Reminders", "Added", reminder.ReminderTime.ToString());
        }
        
        public void AddComment(FocusTaskComment comment)
        {
            Comments.Add(comment);
            comment.Task = this;
            
            // Geçmişe kaydet
            AddHistory("Comments", "Added", comment.Content);
        }
        
        public void AssignTo(Guid assignedToUserId, Guid assignedByUserId, FocusAssignmentRole role)
        {
            var oldUserId = UserId;
            UserId = assignedToUserId;
            
            // Geçmişe kaydet
            AddHistory("UserId", oldUserId.ToString(), assignedToUserId.ToString());
            AddHistory("AssignmentRole", "None", role.ToString());
            
            // Atama kaydı oluştur
            var assignment = new FocusTaskAssignment
            {
                FocusTaskId = Id,
                AssignedToUserId = assignedToUserId,
                AssignedByUserId = assignedByUserId,
                Role = role,
                AssignedAt = DateTime.UtcNow
            };
            
            Assignments.Add(assignment);
        }
        
        public void AddDependency(FocusTask dependencyTask, FocusDependencyType type)
        {
            var dependency = new FocusTaskDependency
            {
                DependentTaskId = Id,
                DependencyTaskId = dependencyTask.Id,
                Type = type
            };
            
            Dependencies.Add(dependency);
            
            // Geçmişe kaydet
            AddHistory("Dependencies", "Added", $"Task: {dependencyTask.Id}, Type: {type}");
        }
        
        private void AddHistory(string fieldName, string oldValue, string newValue)
        {
            var history = new FocusTaskHistory
            {
                FocusTaskId = Id,
                FieldName = fieldName,
                OldValue = oldValue,
                NewValue = newValue,
                ChangedAt = DateTime.UtcNow
            };
            
            History.Add(history);
        }

        public void Delete()
        {
            // Geçmişe kaydet
            AddHistory("Status", Status.ToString(), "Deleted");
            
            // İlişkili nesneleri temizle
            SubTasks.Clear();
            Tags.Clear();
            Reminders.Clear();
            Dependencies.Clear();
            Dependents.Clear();
            Comments.Clear();
            Assignments.Clear();
        }

        public void UpdateStatus(FocusTaskStatus newStatus)
        {
            var oldStatus = Status;
            Status = newStatus;
            
            if (newStatus == FocusTaskStatus.Done)
            {
                CompletedAt = DateTime.UtcNow;
                ProgressPercentage = 100;
            }
            
            // Geçmişe kaydet
            AddHistory("Status", oldStatus.ToString(), newStatus.ToString());
        }
    }
} 