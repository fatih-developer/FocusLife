using FocusLifePlus.Application.Common.Exceptions;
using FocusLifePlus.Application.Common.Interfaces;
using MediatR;

namespace FocusLifePlus.Application.Features.Categories.Commands.DeleteCategory;

public record DeleteCategoryCommand : IRequest
{
    public Guid Id { get; init; }
}

public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand>
{
    private readonly IFocusCategoryRepository _categoryRepository;

    public DeleteCategoryCommandHandler(IFocusCategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.Id);
        if (category == null)
            throw new NotFoundException($"Category with ID {request.Id} not found.");

        var hasSubCategories = await _categoryRepository.HasSubCategoriesAsync(request.Id);
        if (hasSubCategories)
            throw new ValidationException("Cannot delete a category that has sub-categories. Please delete sub-categories first.");

        var hasTasks = await _categoryRepository.HasTasksAsync(request.Id);
        if (hasTasks)
            throw new ValidationException("Cannot delete a category that has tasks. Please remove or reassign tasks first.");

        await _categoryRepository.DeleteAsync(request.Id);
    }
} 