using FocusLifePlus.Application.Features.Categories.Commands.CreateCategory;
using FocusLifePlus.Application.Features.Categories.Commands.DeleteCategory;
using FocusLifePlus.Application.Features.Categories.Commands.UpdateCategory;
using FocusLifePlus.Application.Features.Categories.Queries.GetCategoriesByFilter;
using FocusLifePlus.Application.Features.Categories.Queries.GetCategoryById;
using FocusLifePlus.Application.Features.Categories.Queries.GetCategoryHierarchy;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FocusLifePlus.API.Controllers;

/// <summary>
/// Kategori yönetimi için API endpoint'leri
/// </summary>
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Filtrelere göre kategorileri listeler
    /// </summary>
    /// <param name="searchTerm">Arama terimi</param>
    /// <param name="color">Renk filtresi</param>
    /// <param name="hasTasks">Görevi olan/olmayan kategoriler</param>
    /// <param name="isParentCategory">Ana kategori/alt kategori filtresi</param>
    /// <param name="createdAfter">Bu tarihten sonra oluşturulan kategoriler</param>
    /// <param name="createdBefore">Bu tarihten önce oluşturulan kategoriler</param>
    /// <returns>Filtrelenmiş kategori listesi</returns>
    /// <response code="200">Kategoriler başarıyla getirildi</response>
    /// <response code="401">Yetkisiz erişim</response>
    [HttpGet]
    [ProducesResponseType(typeof(List<CategoryFilterDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<List<CategoryFilterDto>>> GetCategories(
        [FromQuery] string? searchTerm,
        [FromQuery] string? color,
        [FromQuery] bool? hasTasks,
        [FromQuery] bool? isParentCategory,
        [FromQuery] DateTime? createdAfter,
        [FromQuery] DateTime? createdBefore)
    {
        var query = new GetCategoriesByFilterQuery
        {
            UserId = GetCurrentUserId(), // Bu metodu implement etmeniz gerekiyor
            SearchTerm = searchTerm,
            Color = color,
            HasTasks = hasTasks,
            IsParentCategory = isParentCategory,
            CreatedAfter = createdAfter,
            CreatedBefore = createdBefore
        };

        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Belirtilen ID'ye sahip kategoriyi getirir
    /// </summary>
    /// <param name="id">Kategori ID</param>
    /// <returns>Kategori detayları</returns>
    /// <response code="200">Kategori başarıyla getirildi</response>
    /// <response code="401">Yetkisiz erişim</response>
    /// <response code="404">Kategori bulunamadı</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CategoryDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CategoryDetailDto>> GetCategory(Guid id)
    {
        var query = new GetCategoryByIdQuery { Id = id };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Kategori hiyerarşisini getirir
    /// </summary>
    /// <returns>Hiyerarşik kategori listesi</returns>
    /// <response code="200">Kategori hiyerarşisi başarıyla getirildi</response>
    /// <response code="401">Yetkisiz erişim</response>
    [HttpGet("hierarchy")]
    [ProducesResponseType(typeof(List<CategoryHierarchyDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<List<CategoryHierarchyDto>>> GetCategoryHierarchy()
    {
        var query = new GetCategoryHierarchyQuery { UserId = GetCurrentUserId() };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Yeni bir kategori oluşturur
    /// </summary>
    /// <param name="command">Kategori oluşturma bilgileri</param>
    /// <returns>Oluşturulan kategorinin ID'si</returns>
    /// <response code="201">Kategori başarıyla oluşturuldu</response>
    /// <response code="400">Geçersiz istek</response>
    /// <response code="401">Yetkisiz erişim</response>
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<Guid>> CreateCategory(CreateCategoryCommand command)
    {
        command = command with { UserId = GetCurrentUserId() };
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetCategory), new { id = result }, result);
    }

    /// <summary>
    /// Mevcut bir kategoriyi günceller
    /// </summary>
    /// <param name="id">Kategori ID</param>
    /// <param name="command">Güncelleme bilgileri</param>
    /// <returns>İşlem sonucu</returns>
    /// <response code="204">Kategori başarıyla güncellendi</response>
    /// <response code="400">Geçersiz istek</response>
    /// <response code="401">Yetkisiz erişim</response>
    /// <response code="404">Kategori bulunamadı</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCategory(Guid id, UpdateCategoryCommand command)
    {
        if (id != command.Id)
            return BadRequest();

        await _mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Belirtilen kategoriyi siler
    /// </summary>
    /// <param name="id">Kategori ID</param>
    /// <returns>İşlem sonucu</returns>
    /// <response code="204">Kategori başarıyla silindi</response>
    /// <response code="401">Yetkisiz erişim</response>
    /// <response code="404">Kategori bulunamadı</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCategory(Guid id)
    {
        await _mediator.Send(new DeleteCategoryCommand { Id = id });
        return NoContent();
    }

    private Guid GetCurrentUserId()
    {
        // User.Identity.Name veya ClaimTypes.NameIdentifier claim'inden kullanıcı ID'sini alın
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
            throw new UnauthorizedAccessException("User is not authenticated");

        return Guid.Parse(userIdClaim.Value);
    }
} 