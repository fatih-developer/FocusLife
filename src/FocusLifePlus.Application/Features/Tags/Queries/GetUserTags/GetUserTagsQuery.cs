using FocusLifePlus.Application.Common.Interfaces;
using FocusLifePlus.Domain.Entities;
using MediatR;

namespace FocusLifePlus.Application.Features.Tags.Queries.GetUserTags;

public record GetUserTagsQuery : IRequest<List<FocusTagDto>>
{
    public Guid UserId { get; init; }
}

public class GetUserTagsQueryHandler : IRequestHandler<GetUserTagsQuery, List<FocusTagDto>>
{
    private readonly IFocusTagRepository _tagRepository;

    public GetUserTagsQueryHandler(IFocusTagRepository tagRepository)
    {
        _tagRepository = tagRepository;
    }

    public async Task<List<FocusTagDto>> Handle(GetUserTagsQuery request, CancellationToken cancellationToken)
    {
        var tags = await _tagRepository.GetUserTagsAsync(request.UserId);
        return tags.Select(t => new FocusTagDto
        {
            Id = t.Id,
            Name = t.Name,
            Color = t.Color,
            TaskCount = t.Tasks?.Count ?? 0
        }).ToList();
    }
}

public class FocusTagDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Color { get; set; } = null!;
    public int TaskCount { get; set; }
} 