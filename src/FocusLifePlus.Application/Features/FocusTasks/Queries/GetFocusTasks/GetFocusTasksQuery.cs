using System;
using System.Collections.Generic;
using MediatR;
using FocusLifePlus.Application.Features.FocusTasks.Queries.GetFocusTaskById;
using FocusLifePlus.Domain.Common.Enums;

namespace FocusLifePlus.Application.Features.FocusTasks.Queries.GetFocusTasks
{
    public class GetFocusTasksQuery : IRequest<List<FocusTaskDto>>
    {
        public Guid UserId { get; set; }
        public FocusTaskStatus? Status { get; set; }
        public Guid? CategoryId { get; set; }
        public FocusTaskPriority? Priority { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
} 