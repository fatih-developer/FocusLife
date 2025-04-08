using FocusLifePlus.Application.Common.Interfaces;
using FocusLifePlus.Application.Common.Exceptions;
using FocusLifePlus.Domain.Entities;
using MediatR;

namespace FocusLifePlus.Application.Features.Categories.Queries.GetCategoryById;

public record GetCategoryByIdQuery : IRequest<CategoryDetailDto>
{
    public Guid Id { get; init; }
}

public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, CategoryDetailDto>
{
    private readonly IFocusCategoryRepository _categoryRepository;

    public GetCategoryByIdQueryHandler(IFocusCategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<CategoryDetailDto> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.Id);
        if (category == null)
            throw new NotFoundException($"Category with ID {request.Id} not found.");

        var hasSubCategories = await _categoryRepository.HasSubCategoriesAsync(request.Id);
        var hasTasks = await _categoryRepository.HasTasksAsync(request.Id);

        return new CategoryDetailDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            Color = category.Color,
            ParentCategoryId = category.ParentFocusCategoryId,
            HasSubCategories = hasSubCategories,
            HasTasks = hasTasks,
            CreatedAt = category.CreatedAt,
            ModifiedAt = category.ModifiedAt
        };
    }
}

public class CategoryDetailDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Color { get; set; } = null!;
    public Guid? ParentCategoryId { get; set; }
    public bool HasSubCategories { get; set; }
    public bool HasTasks { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
} 