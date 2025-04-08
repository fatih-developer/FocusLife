using System;
using FocusLifePlus.Domain.Common;
using FocusLifePlus.Domain.Common.Enums;

namespace FocusLifePlus.Domain.Entities
{
    public class FocusRecurrenceRule : BaseEntity
    {
        public RecurrenceType Type { get; set; }
        public int Interval { get; set; } // Her X gün/hafta/ay/yıl
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? OccurrenceCount { get; set; }
        public DayOfWeek[] WeekDays { get; set; }
        public int[] MonthDays { get; set; }
        public Guid TaskId { get; set; }
        public FocusTask Task { get; set; }
        public DateTime? LastGeneratedDate { get; set; }
        
        public bool IsActive => EndDate == null || EndDate > DateTime.UtcNow;
        
        public DateTime? GetNextOccurrence(DateTime fromDate)
        {
            if (!IsActive)
                return null;
                
            DateTime nextDate;
            
            switch (Type)
            {
                case RecurrenceType.Daily:
                    nextDate = fromDate.AddDays(Interval);
                    break;
                case RecurrenceType.Weekly:
                    nextDate = fromDate.AddDays(Interval * 7);
                    break;
                case RecurrenceType.Monthly:
                    nextDate = fromDate.AddMonths(Interval);
                    break;
                case RecurrenceType.Yearly:
                    nextDate = fromDate.AddYears(Interval);
                    break;
                default:
                    return null;
            }
            
            if (EndDate.HasValue && nextDate > EndDate.Value)
                return null;
                
            return nextDate;
        }
    }
} 