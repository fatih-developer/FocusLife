using MediatR;
using Microsoft.AspNetCore.Mvc;
using FocusLifePlus.Application.Features.FocusTasks.Commands.CreateFocusTask;
using FocusLifePlus.Application.Features.FocusTasks.Commands.UpdateFocusTask;
using FocusLifePlus.Application.Features.FocusTasks.Commands.DeleteFocusTask;
using FocusLifePlus.Application.Features.FocusTasks.Queries.GetFocusTasks;
using FocusLifePlus.Application.Features.FocusTasks.Queries.GetFocusTaskById;
using Microsoft.AspNetCore.RateLimiting;

namespace FocusLifePlus.API.Controllers;

/// <summary>
/// Controller for managing focus tasks
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class FocusTaskController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<FocusTaskController> _logger;

    public FocusTaskController(IMediator mediator, ILogger<FocusTaskController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Gets all focus tasks with optional filtering
    /// </summary>
    /// <param name="query">Query parameters for filtering tasks</param>
    /// <returns>List of focus tasks</returns>
    /// <response code="200">Returns the list of tasks</response>
    [HttpGet]
    [EnableRateLimiting("fixed")]
    [ProducesResponseType(typeof(List<FocusTaskDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<FocusTaskDto>>> GetFocusTasks([FromQuery] GetFocusTasksQuery query)
    {
        try
        {
            var tasks = await _mediator.Send(query);
            return Ok(tasks);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting focus tasks");
            return StatusCode(500, "An error occurred while processing your request");
        }
    }

    /// <summary>
    /// Gets a specific focus task by id
    /// </summary>
    /// <param name="id">The ID of the task to retrieve</param>
    /// <returns>The requested focus task</returns>
    /// <response code="200">Returns the requested task</response>
    /// <response code="404">If the task is not found</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(FocusTaskDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FocusTaskDto>> GetFocusTaskById(Guid id)
    {
        try
        {
            var task = await _mediator.Send(new GetFocusTaskByIdQuery { Id = id });
            if (task == null)
                return NotFound($"Task with id {id} not found");
            return Ok(task);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting focus task with ID {TaskId}", id);
            return StatusCode(500, "An error occurred while processing your request");
        }
    }

    /// <summary>
    /// Creates a new focus task
    /// </summary>
    /// <param name="command">The task creation command</param>
    /// <returns>The ID of the created task</returns>
    /// <response code="201">Returns the newly created task's ID</response>
    /// <response code="400">If the command is invalid</response>
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Guid>> CreateFocusTask([FromBody] CreateFocusTaskCommand command)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var taskId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetFocusTaskById), new { id = taskId }, taskId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating focus task");
            return StatusCode(500, "An error occurred while processing your request");
        }
    }

    /// <summary>
    /// Updates an existing focus task
    /// </summary>
    /// <param name="id">The ID of the task to update</param>
    /// <param name="command">The task update command</param>
    /// <returns>No content</returns>
    /// <response code="204">If the task was successfully updated</response>
    /// <response code="400">If the command is invalid or ID mismatch</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> UpdateFocusTask(Guid id, [FromBody] UpdateFocusTaskCommand command)
    {
        try
        {
            if (id != command.Id)
                return BadRequest("The ID in the URL must match the ID in the request body");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _mediator.Send(command);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating focus task with ID {TaskId}", id);
            return StatusCode(500, "An error occurred while processing your request");
        }
    }

    /// <summary>
    /// Deletes a focus task
    /// </summary>
    /// <param name="id">The ID of the task to delete</param>
    /// <returns>No content</returns>
    /// <response code="204">If the task was successfully deleted</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> DeleteFocusTask(Guid id)
    {
        try
        {
            await _mediator.Send(new DeleteFocusTaskCommand { Id = id });
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting focus task with ID {TaskId}", id);
            return StatusCode(500, "An error occurred while processing your request");
        }
    }
} 