using MediatR;
using FocusLifePlus.Application.Common.Exceptions;
using FocusLifePlus.Domain.Repositories;

namespace FocusLifePlus.Application.Features.Tasks.Commands.UpdateTask
{
    public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, Unit>
    {
        private readonly IFocusTaskRepository _taskRepository;

        public UpdateTaskCommandHandler(IFocusTaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<Unit> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await _taskRepository.GetByIdAsync(request.Id);

            if (task == null)
            {
                throw new NotFoundException(nameof(Domain.Entities.FocusTask), request.Id);
            }

            task.Update(
                request.Title,
                request.Description,
                request.DueDate,
                request.Priority
            );

            if (request.CategoryId.HasValue)
            {
                task.FocusCategoryId = request.CategoryId.Value;
            }

            if (request.EstimatedDuration.HasValue)
            {
                task.EstimatedDuration = request.EstimatedDuration.Value;
            }

            if (request.ProgressPercentage.HasValue)
            {
                task.UpdateProgress(request.ProgressPercentage.Value);
            }

            if (request.Status.HasValue)
            {
                task.UpdateStatus(request.Status.Value);
            }

            await _taskRepository.UpdateAsync(task);

            return Unit.Value;
        }
    }
} 