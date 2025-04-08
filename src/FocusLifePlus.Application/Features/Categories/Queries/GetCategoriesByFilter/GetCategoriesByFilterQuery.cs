using FocusLifePlus.Application.Common.Interfaces;
using MediatR;

namespace FocusLifePlus.Application.Features.Categories.Queries.GetCategoriesByFilter;

public record GetCategoriesByFilterQuery : IRequest<List<CategoryFilterDto>>
{
    public Guid UserId { get; init; }
    public string? SearchTerm { get; init; }
    public string? Color { get; init; }
    public bool? HasTasks { get; init; }
    public bool? IsParentCategory { get; init; }
    public DateTime? CreatedAfter { get; init; }
    public DateTime? CreatedBefore { get; init; }
}

public class GetCategoriesByFilterQueryHandler : IRequestHandler<GetCategoriesByFilterQuery, List<CategoryFilterDto>>
{
    private readonly IFocusCategoryRepository _categoryRepository;

    public GetCategoriesByFilterQueryHandler(IFocusCategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<List<CategoryFilterDto>> Handle(GetCategoriesByFilterQuery request, CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.FindAsync(c => 
            c.UserId == request.UserId &&
            (!request.SearchTerm.HasValue() || (c.Name.Contains(request.SearchTerm) || c.Description.Contains(request.SearchTerm))) &&
            (!request.Color.HasValue() || c.Color == request.Color) &&
            (!request.IsParentCategory.HasValue || (request.IsParentCategory.Value ? c.ParentFocusCategoryId == null : c.ParentFocusCategoryId != null)) &&
            (!request.CreatedAfter.HasValue || c.CreatedAt >= request.CreatedAfter) &&
            (!request.CreatedBefore.HasValue || c.CreatedAt <= request.CreatedBefore)
        );

        var result = new List<CategoryFilterDto>();

        foreach (var category in categories)
        {
            var hasTasksCondition = !request.HasTasks.HasValue || 
                (request.HasTasks.Value ? await _categoryRepository.HasTasksAsync(category.Id) : !await _categoryRepository.HasTasksAsync(category.Id));

            if (hasTasksCondition)
            {
                result.Add(new CategoryFilterDto
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description,
                    Color = category.Color,
                    ParentCategoryId = category.ParentFocusCategoryId,
                    HasTasks = await _categoryRepository.HasTasksAsync(category.Id),
                    HasSubCategories = await _categoryRepository.HasSubCategoriesAsync(category.Id),
                    CreatedAt = category.CreatedAt
                });
            }
        }

        return result;
    }
}

public class CategoryFilterDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Color { get; set; } = null!;
    public Guid? ParentCategoryId { get; set; }
    public bool HasTasks { get; set; }
    public bool HasSubCategories { get; set; }
    public DateTime CreatedAt { get; set; }
}

public static class StringExtensions
{
    public static bool HasValue(this string? value) => !string.IsNullOrWhiteSpace(value);
} 