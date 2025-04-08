using System;
using System.Collections.Generic;
using MediatR;
using FocusLifePlus.Application.Features.Tasks.DTOs;
using FocusLifePlus.Domain.Common.Enums;

namespace FocusLifePlus.Application.Features.Tasks.Queries.GetUserTasks
{
    public class GetUserTasksQuery : IRequest<List<TaskListDto>>
    {
        public Guid UserId { get; set; }
        public FocusTaskStatus? Status { get; set; }
        public bool? IsOverdue { get; set; }
        public bool? IsDueToday { get; set; }
        public Guid? CategoryId { get; set; }
        public FocusTaskPriority? Priority { get; set; }

        public GetUserTasksQuery(Guid userId)
        {
            UserId = userId;
        }
    }
} 