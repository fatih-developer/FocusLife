using FocusLifePlus.Application.Common.Interfaces;
using FocusLifePlus.Domain.Entities;
using MediatR;

namespace FocusLifePlus.Application.Features.Categories.Queries.GetUserCategories;

public record GetUserCategoriesQuery : IRequest<List<FocusCategoryDto>>
{
    public Guid UserId { get; init; }
}

public class GetUserCategoriesQueryHandler : IRequestHandler<GetUserCategoriesQuery, List<FocusCategoryDto>>
{
    private readonly IFocusCategoryRepository _categoryRepository;

    public GetUserCategoriesQueryHandler(IFocusCategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<List<FocusCategoryDto>> Handle(GetUserCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.GetUserCategoriesAsync(request.UserId);
        return categories.Select(c => new FocusCategoryDto
        {
            Id = c.Id,
            Name = c.Name,
            Description = c.Description,
            Color = c.Color,
            ParentCategoryId = c.ParentFocusCategoryId,
            HasSubCategories = c.SubCategories?.Any() ?? false,
            HasTasks = c.Tasks?.Any() ?? false
        }).ToList();
    }
}

public class FocusCategoryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Color { get; set; } = null!;
    public Guid? ParentCategoryId { get; set; }
    public bool HasSubCategories { get; set; }
    public bool HasTasks { get; set; }
} 