using System;
using FocusLifePlus.Domain.Common;

namespace FocusLifePlus.Domain.Entities;

public class FocusTaskComment : BaseEntity
{
    public string Content { get; set; }
    public DateTime CommentDate { get; set; }
    public Guid TaskId { get; set; }
    public FocusTask Task { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
} 