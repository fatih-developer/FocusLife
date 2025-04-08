using System;
using MediatR;
using FocusLifePlus.Application.Features.Tasks.DTOs;

namespace FocusLifePlus.Application.Features.Tasks.Queries.GetTaskById
{
    public class GetTaskByIdQuery : IRequest<TaskDetailsDto>
    {
        public Guid Id { get; set; }

        public GetTaskByIdQuery(Guid id)
        {
            Id = id;
        }
    }
} 