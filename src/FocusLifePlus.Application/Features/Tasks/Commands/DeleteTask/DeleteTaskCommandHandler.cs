using MediatR;
using FocusLifePlus.Domain.Repositories;
using FocusLifePlus.Application.Common.Exceptions;

namespace FocusLifePlus.Application.Features.Tasks.Commands.DeleteTask
{
    public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, Unit>
    {
        private readonly IFocusTaskRepository _taskRepository;

        public DeleteTaskCommandHandler(IFocusTaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<Unit> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await _taskRepository.GetByIdAsync(request.Id);

            if (task == null)
            {
                throw new NotFoundException(nameof(Domain.Entities.FocusTask), request.Id);
            }

            //task.Delete();
            await _taskRepository.DeleteAsync(task);
            return Unit.Value;
        }
    }
} 