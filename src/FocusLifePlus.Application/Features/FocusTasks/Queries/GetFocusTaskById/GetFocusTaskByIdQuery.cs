using System;
using MediatR;
using FocusLifePlus.Application.Features.FocusTasks.Queries.GetFocusTaskById;

namespace FocusLifePlus.Application.Features.FocusTasks.Queries.GetFocusTaskById
{
    public class GetFocusTaskByIdQuery : IRequest<FocusTaskDto>
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
    }
} 