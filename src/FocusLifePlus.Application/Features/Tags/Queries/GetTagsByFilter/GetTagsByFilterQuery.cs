using FocusLifePlus.Application.Common.Interfaces;
using MediatR;

namespace FocusLifePlus.Application.Features.Tags.Queries.GetTagsByFilter;

public record GetTagsByFilterQuery : IRequest<List<TagFilterDto>>
{
    public Guid UserId { get; init; }
    public string? SearchTerm { get; init; }
    public string? Color { get; init; }
    public bool? HasTasks { get; init; }
    public DateTime? CreatedAfter { get; init; }
    public DateTime? CreatedBefore { get; init; }
    public int? MinimumTaskCount { get; init; }
}

public class GetTagsByFilterQueryHandler : IRequestHandler<GetTagsByFilterQuery, List<TagFilterDto>>
{
    private readonly IFocusTagRepository _tagRepository;

    public GetTagsByFilterQueryHandler(IFocusTagRepository tagRepository)
    {
        _tagRepository = tagRepository;
    }

    public async Task<List<TagFilterDto>> Handle(GetTagsByFilterQuery request, CancellationToken cancellationToken)
    {
        var tags = await _tagRepository.GetUserTagsAsync(request.UserId);

        var filteredTags = tags.Where(t =>
            (!request.SearchTerm.HasValue() || t.Name.Contains(request.SearchTerm)) &&
            (!request.Color.HasValue() || t.Color == request.Color) &&
            (!request.CreatedAfter.HasValue || t.CreatedAt >= request.CreatedAfter) &&
            (!request.CreatedBefore.HasValue || t.CreatedAt <= request.CreatedBefore) &&
            (!request.MinimumTaskCount.HasValue || (t.Tasks?.Count ?? 0) >= request.MinimumTaskCount)
        ).ToList();

        var result = new List<TagFilterDto>();

        foreach (var tag in filteredTags)
        {
            var hasTasksCondition = !request.HasTasks.HasValue ||
                (request.HasTasks.Value ? await _tagRepository.HasTasksAsync(tag.Id) : !await _tagRepository.HasTasksAsync(tag.Id));

            if (hasTasksCondition)
            {
                result.Add(new TagFilterDto
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    Color = tag.Color,
                    TaskCount = tag.Tasks?.Count ?? 0,
                    CreatedAt = tag.CreatedAt
                });
            }
        }

        return result;
    }
}

public class TagFilterDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Color { get; set; } = null!;
    public int TaskCount { get; set; }
    public DateTime CreatedAt { get; set; }
}

public static class StringExtensions
{
    public static bool HasValue(this string? value) => !string.IsNullOrWhiteSpace(value);
} 