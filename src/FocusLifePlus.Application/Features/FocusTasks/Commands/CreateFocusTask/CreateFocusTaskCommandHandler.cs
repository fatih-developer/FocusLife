using FocusLifePlus.Domain.Entities;
using FocusLifePlus.Domain.Repositories;
using MediatR;

namespace FocusLifePlus.Application.Features.FocusTasks.Commands.CreateFocusTask
{
    public class CreateFocusTaskCommandHandler : IRequestHandler<CreateFocusTaskCommand, Guid>
    {
        private readonly IFocusTaskRepository _taskRepository;

        public CreateFocusTaskCommandHandler(IFocusTaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<Guid> Handle(CreateFocusTaskCommand request, CancellationToken cancellationToken)
        {
            var task = new FocusTask
            {
                Title = request.Title,
                Description = request.Description,
                DueDate = request.DueDate,
                Priority = request.Priority,
                FocusCategoryId = request.CategoryId,
                UserId = request.UserId,
                EstimatedDuration = request.EstimatedDuration
            };

            var createdTask = await _taskRepository.AddAsync(task);
            return createdTask.Id;
        }
    }
} 