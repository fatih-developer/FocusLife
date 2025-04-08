using FocusLifePlus.Application.Common.Interfaces;
using FocusLifePlus.Domain.Entities;
using MediatR;

namespace FocusLifePlus.Application.Features.Tags.Commands.CreateTag;

public record CreateTagCommand : IRequest<Guid>
{
    public string Name { get; init; } = null!;
    public string Color { get; init; } = null!;
    public Guid UserId { get; init; }
}

public class CreateTagCommandHandler : IRequestHandler<CreateTagCommand, Guid>
{
    private readonly IFocusTagRepository _tagRepository;

    public CreateTagCommandHandler(IFocusTagRepository tagRepository)
    {
        _tagRepository = tagRepository;
    }

    public async Task<Guid> Handle(CreateTagCommand request, CancellationToken cancellationToken)
    {
        var tag = new FocusTag
        {
            Name = request.Name,
            Color = request.Color,
            UserId = request.UserId,
            Tasks = new List<FocusTask>()
        };

        var result = await _tagRepository.AddAsync(tag);
        return result.Id;
    }
} 