using System.Security.Claims;
using FocusLifePlus.Application.Features.Tags.Commands.CreateTag;
using FocusLifePlus.Application.Features.Tags.Commands.DeleteTag;
using FocusLifePlus.Application.Features.Tags.Commands.UpdateTag;
using FocusLifePlus.Application.Features.Tags.Queries.GetTagsByFilter;
using FocusLifePlus.Application.Features.Tags.Queries.GetTagsByTask;
using FocusLifePlus.Application.Features.Tags.Queries.GetTagStatistics;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FocusLifePlus.API.Controllers;

/// <summary>
/// Etiket yönetimi için API endpoint'leri
/// </summary>
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TagsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TagsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Filtrelere göre etiketleri listeler
    /// </summary>
    /// <param name="searchTerm">Arama terimi</param>
    /// <param name="color">Renk filtresi</param>
    /// <param name="hasTasks">Görevi olan/olmayan etiketler</param>
    /// <param name="createdAfter">Bu tarihten sonra oluşturulan etiketler</param>
    /// <param name="createdBefore">Bu tarihten önce oluşturulan etiketler</param>
    /// <param name="minimumTaskCount">Minimum görev sayısı</param>
    /// <returns>Filtrelenmiş etiket listesi</returns>
    /// <response code="200">Etiketler başarıyla getirildi</response>
    /// <response code="401">Yetkisiz erişim</response>
    [HttpGet]
    [ProducesResponseType(typeof(List<TagFilterDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<List<TagFilterDto>>> GetTags(
        [FromQuery] string? searchTerm,
        [FromQuery] string? color,
        [FromQuery] bool? hasTasks,
        [FromQuery] DateTime? createdAfter,
        [FromQuery] DateTime? createdBefore,
        [FromQuery] int? minimumTaskCount)
    {
        var query = new GetTagsByFilterQuery
        {
            UserId = GetCurrentUserId(),
            SearchTerm = searchTerm,
            Color = color,
            HasTasks = hasTasks,
            CreatedAfter = createdAfter,
            CreatedBefore = createdBefore,
            MinimumTaskCount = minimumTaskCount
        };

        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Belirtilen görevin etiketlerini getirir
    /// </summary>
    /// <param name="taskId">Görev ID</param>
    /// <returns>Görevin etiket listesi</returns>
    /// <response code="200">Etiketler başarıyla getirildi</response>
    /// <response code="401">Yetkisiz erişim</response>
    /// <response code="404">Görev bulunamadı</response>
    [HttpGet("task/{taskId}")]
    [ProducesResponseType(typeof(List<TaskTagDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<TaskTagDto>>> GetTagsByTask(Guid taskId)
    {
        var query = new GetTagsByTaskQuery { TaskId = taskId };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("statistics")]
    public async Task<ActionResult<TagStatisticsDto>> GetTagStatistics()
    {
        var query = new GetTagStatisticsQuery { UserId = GetCurrentUserId() };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateTag(CreateTagCommand command)
    {
        command = command with { UserId = GetCurrentUserId() };
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetTagsByTask), new { taskId = result }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTag(Guid id, UpdateTagCommand command)
    {
        if (id != command.Id)
            return BadRequest();

        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTag(Guid id)
    {
        await _mediator.Send(new DeleteTagCommand { Id = id });
        return NoContent();
    }

    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
            throw new UnauthorizedAccessException("User is not authenticated");

        return Guid.Parse(userIdClaim.Value);
    }
} 