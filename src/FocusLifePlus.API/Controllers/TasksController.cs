using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using MediatR;
using FocusLifePlus.Application.Features.Tasks.Commands.CreateTask;
using FocusLifePlus.Application.Features.Tasks.Commands.UpdateTask;
using FocusLifePlus.Application.Features.Tasks.Commands.DeleteTask;
using FocusLifePlus.Application.Features.Tasks.Queries.GetTaskById;
using FocusLifePlus.Application.Features.Tasks.Queries.GetUserTasks;
using FocusLifePlus.Application.Features.Tasks.DTOs;
using FocusLifePlus.Domain.Common.Enums;
using FocusLifePlus.API.Extensions;

namespace FocusLifePlus.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TasksController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Yeni bir görev oluşturur
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateTaskCommand command)
        {
            var taskId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = taskId }, taskId);
        }

        /// <summary>
        /// Belirtilen ID'ye sahip görevi getirir
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TaskDetailsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TaskDetailsDto>> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetTaskByIdQuery(id));
            return Ok(result);
        }

        /// <summary>
        /// Kullanıcının görevlerini filtreli olarak getirir
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<TaskListDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<TaskListDto>>> GetUserTasks(
            [FromQuery] FocusTaskStatus? status,
            [FromQuery] bool? isOverdue,
            [FromQuery] bool? isDueToday,
            [FromQuery] Guid? categoryId,
            [FromQuery] FocusTaskPriority? priority)
        {
            // TODO: Gerçek kullanıcı ID'sini auth sisteminden al
            var userId = User.GetUserId();
            
            var query = new GetUserTasksQuery(userId)
            {
                Status = status,
                IsOverdue = isOverdue,
                IsDueToday = isDueToday,
                CategoryId = categoryId,
                Priority = priority
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Belirtilen görevi günceller
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTaskCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }

            await _mediator.Send(command);
            return NoContent();
        }

        /// <summary>
        /// Belirtilen görevi siler
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteTaskCommand(id));
            return NoContent();
        }

        /// <summary>
        /// Görevin durumunu günceller
        /// </summary>
        [HttpPatch("{id}/status")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] FocusTaskStatus status)
        {
            var command = new UpdateTaskCommand { Id = id, Status = status };
            await _mediator.Send(command);
            return NoContent();
        }

        /// <summary>
        /// Görevin ilerleme yüzdesini günceller
        /// </summary>
        [HttpPatch("{id}/progress")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateProgress(Guid id, [FromBody] int progressPercentage)
        {
            var command = new UpdateTaskCommand { Id = id, ProgressPercentage = progressPercentage };
            await _mediator.Send(command);
            return NoContent();
        }

        /// <summary>
        /// Bugün yapılması gereken görevleri getirir
        /// </summary>
        [HttpGet("due-today")]
        [ProducesResponseType(typeof(List<TaskListDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<TaskListDto>>> GetTasksDueToday()
        {
            var userId = User.GetUserId();
            var query = new GetUserTasksQuery(userId) { IsDueToday = true };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Gecikmiş görevleri getirir
        /// </summary>
        [HttpGet("overdue")]
        [ProducesResponseType(typeof(List<TaskListDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<TaskListDto>>> GetOverdueTasks()
        {
            var userId = User.GetUserId();
            var query = new GetUserTasksQuery(userId) { IsOverdue = true };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
} 