using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using FocusLifePlus.Domain.Repositories;
using FocusLifePlus.Application.Common.Exceptions;
using FocusLifePlus.Application.Features.FocusTasks.Commands.UpdateFocusTask;
using FocusLifePlus.Domain.Entities;

namespace FocusLifePlus.Application.Features.FocusTasks.Commands.UpdateFocusTask
{
    public class UpdateFocusTaskCommandHandler : IRequestHandler<UpdateFocusTaskCommand, Unit>
    {
        private readonly IFocusTaskRepository _taskRepository;

        public UpdateFocusTaskCommandHandler(IFocusTaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<Unit> Handle(UpdateFocusTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await _taskRepository.GetByIdAsync(request.Id);
            
            if (task == null)
                throw new NotFoundException(nameof(FocusTask), request.Id);

            task.Update(request.Title, request.Description, request.DueDate, request.Priority);
            
            if (request.CategoryId.HasValue)
                task.FocusCategoryId = request.CategoryId.Value;
                
            if (request.EstimatedDuration.HasValue)
                task.EstimatedDuration = request.EstimatedDuration.Value;
                
            if (request.ActualDuration.HasValue)
                task.ActualDuration = request.ActualDuration.Value;
                
            if (request.ProgressPercentage.HasValue)
                task.UpdateProgress(request.ProgressPercentage.Value);

            await _taskRepository.UpdateAsync(task);
            
            return Unit.Value;
        }
    }
} 