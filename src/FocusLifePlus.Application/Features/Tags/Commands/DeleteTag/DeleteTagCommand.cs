using FocusLifePlus.Application.Common.Exceptions;
using FocusLifePlus.Application.Common.Interfaces;
using MediatR;

namespace FocusLifePlus.Application.Features.Tags.Commands.DeleteTag;

public record DeleteTagCommand : IRequest
{
    public Guid Id { get; init; }
}

public class DeleteTagCommandHandler : IRequestHandler<DeleteTagCommand>
{
    private readonly IFocusTagRepository _tagRepository;

    public DeleteTagCommandHandler(IFocusTagRepository tagRepository)
    {
        _tagRepository = tagRepository;
    }

    public async Task Handle(DeleteTagCommand request, CancellationToken cancellationToken)
    {
        var tag = await _tagRepository.GetByIdAsync(request.Id);
        if (tag == null)
            throw new NotFoundException($"Tag with ID {request.Id} not found.");

        var hasTasks = await _tagRepository.HasTasksAsync(request.Id);
        if (hasTasks)
            throw new ValidationException("Cannot delete a tag that is assigned to tasks. Please remove tag from tasks first.");

        await _tagRepository.DeleteAsync(request.Id);
    }
} 