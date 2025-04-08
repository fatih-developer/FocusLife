using FocusLifePlus.Application.Common.Interfaces;
using MediatR;

namespace FocusLifePlus.Application.Features.Tags.Queries.GetTagStatistics;

public record GetTagStatisticsQuery : IRequest<TagStatisticsDto>
{
    public Guid UserId { get; init; }
}

public class GetTagStatisticsQueryHandler : IRequestHandler<GetTagStatisticsQuery, TagStatisticsDto>
{
    private readonly IFocusTagRepository _tagRepository;

    public GetTagStatisticsQueryHandler(IFocusTagRepository tagRepository)
    {
        _tagRepository = tagRepository;
    }

    public async Task<TagStatisticsDto> Handle(GetTagStatisticsQuery request, CancellationToken cancellationToken)
    {
        var tags = await _tagRepository.GetUserTagsAsync(request.UserId);

        return new TagStatisticsDto
        {
            TotalTags = tags.Count,
            TagsWithTasks = tags.Count(t => t.Tasks?.Any() ?? false),
            TagsWithoutTasks = tags.Count(t => !t.Tasks?.Any() ?? true),
            MostUsedTags = tags.OrderByDescending(t => t.Tasks?.Count ?? 0)
                .Take(5)
                .Select(t => new TagUsageDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    Color = t.Color,
                    TaskCount = t.Tasks?.Count ?? 0
                }).ToList(),
            RecentlyCreatedTags = tags.OrderByDescending(t => t.CreatedAt)
                .Take(5)
                .Select(t => new RecentTagDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    Color = t.Color,
                    CreatedAt = t.CreatedAt
                }).ToList()
        };
    }
}

public class TagStatisticsDto
{
    public int TotalTags { get; set; }
    public int TagsWithTasks { get; set; }
    public int TagsWithoutTasks { get; set; }
    public List<TagUsageDto> MostUsedTags { get; set; } = new();
    public List<RecentTagDto> RecentlyCreatedTags { get; set; } = new();
}

public class TagUsageDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Color { get; set; } = null!;
    public int TaskCount { get; set; }
}

public class RecentTagDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Color { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
} 