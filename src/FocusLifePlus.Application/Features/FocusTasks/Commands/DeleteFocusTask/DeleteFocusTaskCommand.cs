using System;
using MediatR;

namespace FocusLifePlus.Application.Features.FocusTasks.Commands.DeleteFocusTask
{
    public class DeleteFocusTaskCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }
} 