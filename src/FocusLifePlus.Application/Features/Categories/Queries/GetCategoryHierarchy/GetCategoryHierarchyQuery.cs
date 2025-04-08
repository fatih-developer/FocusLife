using FocusLifePlus.Application.Common.Interfaces;
using FocusLifePlus.Domain.Entities;
using MediatR;

namespace FocusLifePlus.Application.Features.Categories.Queries.GetCategoryHierarchy;

public record GetCategoryHierarchyQuery : IRequest<List<CategoryHierarchyDto>>
{
    public Guid UserId { get; init; }
}

public class GetCategoryHierarchyQueryHandler : IRequestHandler<GetCategoryHierarchyQuery, List<CategoryHierarchyDto>>
{
    private readonly IFocusCategoryRepository _categoryRepository;

    public GetCategoryHierarchyQueryHandler(IFocusCategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<List<CategoryHierarchyDto>> Handle(GetCategoryHierarchyQuery request, CancellationToken cancellationToken)
    {
        var allCategories = await _categoryRepository.GetUserCategoriesAsync(request.UserId);
        var rootCategories = allCategories.Where(c => c.ParentFocusCategoryId == null).ToList();

        var result = new List<CategoryHierarchyDto>();
        foreach (var rootCategory in rootCategories)
        {
            result.Add(await BuildCategoryHierarchy(rootCategory));
        }

        return result;
    }

    private async Task<CategoryHierarchyDto> BuildCategoryHierarchy(FocusCategory category)
    {
        var subCategories = await _categoryRepository.GetSubCategoriesAsync(category.Id);
        var subCategoryDtos = new List<CategoryHierarchyDto>();

        foreach (var subCategory in subCategories)
        {
            subCategoryDtos.Add(await BuildCategoryHierarchy(subCategory));
        }

        return new CategoryHierarchyDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            Color = category.Color,
            HasTasks = await _categoryRepository.HasTasksAsync(category.Id),
            Level = category.ParentFocusCategoryId.HasValue ? await GetCategoryLevel(category) : 0,
            SubCategories = subCategoryDtos
        };
    }

    private async Task<int> GetCategoryLevel(FocusCategory category)
    {
        var level = 0;
        var currentCategory = category;

        while (currentCategory.ParentFocusCategoryId.HasValue)
        {
            level++;
            currentCategory = await _categoryRepository.GetByIdAsync(currentCategory.ParentFocusCategoryId.Value);
            if (currentCategory == null) break;
        }

        return level;
    }
}

public class CategoryHierarchyDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Color { get; set; } = null!;
    public bool HasTasks { get; set; }
    public int Level { get; set; }
    public List<CategoryHierarchyDto> SubCategories { get; set; } = new();
} 