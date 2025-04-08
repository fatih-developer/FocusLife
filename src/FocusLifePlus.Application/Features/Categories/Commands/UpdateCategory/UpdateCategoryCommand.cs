using FocusLifePlus.Application.Common.Interfaces;
using FocusLifePlus.Application.Common.Exceptions;
using FocusLifePlus.Domain.Entities;
using MediatR;

namespace FocusLifePlus.Application.Features.Categories.Commands.UpdateCategory;

public record UpdateCategoryCommand : IRequest
{
    public Guid Id { get; init; }
    public string Name { get; init; } = null!;
    public string Description { get; init; } = null!;
    public string Color { get; init; } = null!;
    public Guid? ParentCategoryId { get; init; }
}

public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand>
{
    private readonly IFocusCategoryRepository _categoryRepository;

    public UpdateCategoryCommandHandler(IFocusCategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.Id);
        if (category == null)
            throw new NotFoundException($"Category with ID {request.Id} not found.");

        if (request.ParentCategoryId.HasValue)
        {
            if (request.ParentCategoryId == request.Id)
                throw new ValidationException("A category cannot be its own parent.");

            var exists = await _categoryRepository.ExistsAsync(request.ParentCategoryId.Value);
            if (!exists)
                throw new NotFoundException($"Parent category with ID {request.ParentCategoryId} not found.");
        }

        category.Name = request.Name;
        category.Description = request.Description;
        category.Color = request.Color;
        category.ParentFocusCategoryId = request.ParentCategoryId;

        await _categoryRepository.UpdateAsync(category);
    }
} 