using FocusLifePlus.Application.Common.Interfaces;
using FocusLifePlus.Application.Common.Exceptions;
using FocusLifePlus.Domain.Entities;
using MediatR;

namespace FocusLifePlus.Application.Features.Tags.Queries.GetTagById;

public record GetTagByIdQuery : IRequest<TagDetailDto>
{
    public Guid Id { get; init; }
}

public class GetTagByIdQueryHandler : IRequestHandler<GetTagByIdQuery, TagDetailDto>
{
    private readonly IFocusTagRepository _tagRepository;

    public GetTagByIdQueryHandler(IFocusTagRepository tagRepository)
    {
        _tagRepository = tagRepository;
    }

    public async Task<TagDetailDto> Handle(GetTagByIdQuery request, CancellationToken cancellationToken)
    {
        var tag = await _tagRepository.GetByIdAsync(request.Id);
        if (tag == null)
        {
            throw new NotFoundException(nameof(FocusTag), request.Id);
        }

        return new TagDetailDto
        {
            Id = tag.Id,
            Name = tag.Name,
            Color = tag.Color,
            TaskCount = tag.Tasks?.Count ?? 0,
            CreatedAt = tag.CreatedAt,
            ModifiedAt = tag.ModifiedAt
        };
    }
}

public class TagDetailDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Color { get; set; } = null!;
    public int TaskCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
} 