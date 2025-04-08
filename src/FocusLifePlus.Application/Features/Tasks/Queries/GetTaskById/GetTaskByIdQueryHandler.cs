using AutoMapper;
using FocusLifePlus.Application.Common.Exceptions;
using FocusLifePlus.Application.Features.Tasks.DTOs;
using FocusLifePlus.Domain.Repositories;
using MediatR;


namespace FocusLifePlus.Application.Features.Tasks.Queries.GetTaskById
{
    public class GetTaskByIdQueryHandler : IRequestHandler<GetTaskByIdQuery, TaskDetailsDto>
    {
        private readonly IFocusTaskRepository _taskRepository;
        private readonly IMapper _mapper;

        public GetTaskByIdQueryHandler(IFocusTaskRepository taskRepository, IMapper mapper)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
        }

        public async Task<TaskDetailsDto> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
        {
            var task = await _taskRepository.GetByIdAsync(request.Id);

            if (task == null)
            {
                throw new NotFoundException(nameof(Domain.Entities.FocusTask), request.Id);
            }

            return _mapper.Map<TaskDetailsDto>(task);
        }
    }
} 