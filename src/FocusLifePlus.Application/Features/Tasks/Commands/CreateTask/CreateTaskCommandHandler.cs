using MediatR;
using FocusLifePlus.Domain.Entities;
using FocusLifePlus.Domain.Repositories;

namespace FocusLifePlus.Application.Features.Tasks.Commands.CreateTask
{
    public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, Guid>
    {
        private readonly IFocusTaskRepository _taskRepository;

        public CreateTaskCommandHandler(IFocusTaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<Guid> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            var task = new FocusTask
            {
                Title = request.Title,
                Description = request.Description,
                DueDate = request.DueDate,
                Priority = request.Priority,
                UserId = request.UserId
            };

            if (request.CategoryId.HasValue)
            {
                task.FocusCategoryId = request.CategoryId.Value;
            }

            if (request.EstimatedDuration.HasValue)
            {
                task.EstimatedDuration = request.EstimatedDuration.Value;
            }

            var createdTask = await _taskRepository.AddAsync(task);
            return createdTask.Id;
        }
    }
} 