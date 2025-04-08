using FocusLifePlus.Application.Common.Interfaces;
using FocusLifePlus.Application.Common.Exceptions;
using FocusLifePlus.Domain.Entities;
using MediatR;

namespace FocusLifePlus.Application.Features.Tags.Commands.UpdateTag;

public record UpdateTagCommand : IRequest
{
    public Guid Id { get; init; }
    public string Name { get; init; } = null!;
    public string Color { get; init; } = null!;
}

public class UpdateTagCommandHandler : IRequestHandler<UpdateTagCommand>
{
    private readonly IFocusTagRepository _tagRepository;

    public UpdateTagCommandHandler(IFocusTagRepository tagRepository)
    {
        _tagRepository = tagRepository;
    }

    public async Task Handle(UpdateTagCommand request, CancellationToken cancellationToken)
    {
        var tag = await _tagRepository.GetByIdAsync(request.Id);
        if (tag == null)
            throw new NotFoundException($"Tag with ID {request.Id} not found.");

        tag.Name = request.Name;
        tag.Color = request.Color;

        await _tagRepository.UpdateAsync(tag);
    }
} 