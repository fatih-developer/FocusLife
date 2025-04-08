using AutoMapper;
using MediatR;
using FocusLifePlus.Application.Features.Tasks.DTOs;
using FocusLifePlus.Domain.Repositories;

namespace FocusLifePlus.Application.Features.Tasks.Queries.GetUserTasks
{
    public class GetUserTasksQueryHandler : IRequestHandler<GetUserTasksQuery, List<TaskListDto>>
    {
        private readonly IFocusTaskRepository _taskRepository;
        private readonly IMapper _mapper;

        public GetUserTasksQueryHandler(IFocusTaskRepository taskRepository, IMapper mapper)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
        }

        public async Task<List<TaskListDto>> Handle(GetUserTasksQuery request, CancellationToken cancellationToken)
        {
            var tasks = await _taskRepository.GetTasksByUserIdAsync(request.UserId);


            // Filtreleme
            var filteredTasks = tasks.AsQueryable();

            if (request.Status.HasValue)
            {
                filteredTasks = filteredTasks.Where(t => t.Status == request.Status.Value);
            }

            if (request.IsOverdue.HasValue && request.IsOverdue.Value)
            {
                filteredTasks = filteredTasks.Where(t => t.IsOverdue);
            }

            if (request.IsDueToday.HasValue && request.IsDueToday.Value)
            {
                filteredTasks = filteredTasks.Where(t => t.IsDueToday);
            }

            if (request.CategoryId.HasValue)
            {
                filteredTasks = filteredTasks.Where(t => t.FocusCategoryId == request.CategoryId.Value);
            }

            if (request.Priority.HasValue)
            {
                filteredTasks = filteredTasks.Where(t => t.Priority == request.Priority.Value);
            }

            return _mapper.Map<List<TaskListDto>>(filteredTasks.ToList());
        }
    }
} 