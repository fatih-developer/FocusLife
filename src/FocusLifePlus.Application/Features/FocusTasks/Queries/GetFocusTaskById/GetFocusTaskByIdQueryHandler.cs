using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using FocusLifePlus.Application.Common.Interfaces;
using FocusLifePlus.Application.Common.Exceptions;
using FocusLifePlus.Application.Features.FocusTasks.Queries.GetFocusTaskById;
using FocusLifePlus.Domain.Entities;

namespace FocusLifePlus.Application.Features.FocusTasks.Queries.GetFocusTaskById
{
    public class GetFocusTaskByIdQueryHandler : IRequestHandler<GetFocusTaskByIdQuery, FocusTaskDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetFocusTaskByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<FocusTaskDto> Handle(GetFocusTaskByIdQuery request, CancellationToken cancellationToken)
        {
            var task = await _context.FocusTasks
                .Include(t => t.FocusCategory)
                .FirstOrDefaultAsync(t => t.Id == request.Id && t.UserId == request.UserId, cancellationToken);

            if (task == null)
            {
                throw new NotFoundException(nameof(FocusTask), request.Id);
            }

            var taskDto = _mapper.Map<FocusTaskDto>(task);
            taskDto.CategoryName = task.FocusCategory.Name;

            return taskDto;
        }
    }
} 