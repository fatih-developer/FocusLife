using FocusLifePlus.Application.Common.Interfaces;
using FocusLifePlus.Application.Common.Exceptions;
using FocusLifePlus.Domain.Entities;
using MediatR;

namespace FocusLifePlus.Application.Features.Categories.Commands.CreateCategory;

public record CreateCategoryCommand : IRequest<Guid>
{
    public string Name { get; init; } = null!;
    public string Description { get; init; } = null!;
    public string Color { get; init; } = null!;
    public Guid? ParentCategoryId { get; init; }
    public Guid UserId { get; init; }
}

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Guid>
{
    private readonly IFocusCategoryRepository _categoryRepository;

    public CreateCategoryCommandHandler(IFocusCategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Guid> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        if (request.ParentCategoryId.HasValue)
        {
            var exists = await _categoryRepository.ExistsAsync(request.ParentCategoryId.Value);
            if (!exists)
                throw new NotFoundException($"Parent category with ID {request.ParentCategoryId} not found.");
        }

        var category = new FocusCategory
        {
            Name = request.Name,
            Description = request.Description,
            Color = request.Color,
            ParentFocusCategoryId = request.ParentCategoryId,
            UserId = request.UserId
        };

        var result = await _categoryRepository.AddAsync(category);
        return result.Id;
    }
} 