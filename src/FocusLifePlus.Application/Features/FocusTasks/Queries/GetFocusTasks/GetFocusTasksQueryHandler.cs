using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using FocusLifePlus.Application.Common.Interfaces;
using FocusLifePlus.Domain.Entities;
using FocusLifePlus.Application.Features.FocusTasks.Queries.GetFocusTaskById;
using FocusLifePlus.Application.Features.FocusTasks.Queries.GetFocusTasks;

namespace FocusLifePlus.Application.Features.FocusTasks.Queries.GetFocusTasks
{
    public class GetFocusTasksQueryHandler : IRequestHandler<GetFocusTasksQuery, List<FocusTaskDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetFocusTasksQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<FocusTaskDto>> Handle(GetFocusTasksQuery request, CancellationToken cancellationToken)
        {
            var query = _context.FocusTasks
                .Include(t => t.FocusCategory)
                .Where(t => t.UserId == request.UserId);

            if (request.Status.HasValue)
            {
                query = query.Where(t => t.Status == request.Status.Value);
            }

            if (request.CategoryId.HasValue)
            {
                query = query.Where(t => t.FocusCategoryId == request.CategoryId.Value);
            }

            if (request.Priority.HasValue)
            {
                query = query.Where(t => t.Priority == request.Priority.Value);
            }

            if (request.StartDate.HasValue)
            {
                query = query.Where(t => t.DueDate >= request.StartDate.Value);
            }

            if (request.EndDate.HasValue)
            {
                query = query.Where(t => t.DueDate <= request.EndDate.Value);
            }

            var tasks = await query.ToListAsync(cancellationToken);
            return _mapper.Map<List<FocusTaskDto>>(tasks);
        }
    }
} 