using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using FocusLifePlus.Domain.Repositories;
using FocusLifePlus.Application.Common.Exceptions;
using FocusLifePlus.Application.Features.FocusTasks.Commands.DeleteFocusTask;
using FocusLifePlus.Domain.Entities;

namespace FocusLifePlus.Application.Features.FocusTasks.Commands.DeleteFocusTask
{
    public class DeleteFocusTaskCommandHandler : IRequestHandler<DeleteFocusTaskCommand, Unit>
    {
        private readonly IFocusTaskRepository _taskRepository;

        public DeleteFocusTaskCommandHandler(IFocusTaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<Unit> Handle(DeleteFocusTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await _taskRepository.GetByIdAsync(request.Id);
            
            if (task == null)
                throw new NotFoundException(nameof(FocusTask), request.Id);

            await _taskRepository.DeleteAsync(task);
            
            return Unit.Value;
        }
    }
} 