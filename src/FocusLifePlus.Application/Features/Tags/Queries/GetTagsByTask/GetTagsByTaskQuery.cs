using FocusLifePlus.Application.Common.Interfaces;
using MediatR;

namespace FocusLifePlus.Application.Features.Tags.Queries.GetTagsByTask;

public record GetTagsByTaskQuery : IRequest<List<TaskTagDto>>
{
    public Guid TaskId { get; init; }
}

public class GetTagsByTaskQueryHandler : IRequestHandler<GetTagsByTaskQuery, List<TaskTagDto>>
{
    private readonly IFocusTagRepository _tagRepository;

    public GetTagsByTaskQueryHandler(IFocusTagRepository tagRepository)
    {
        _tagRepository = tagRepository;
    }

    public async Task<List<TaskTagDto>> Handle(GetTagsByTaskQuery request, CancellationToken cancellationToken)
    {
        var tags = await _tagRepository.GetTagsByTaskAsync(request.TaskId);

        return tags.Select(t => new TaskTagDto
        {
            Id = t.Id,
            Name = t.Name,
            Color = t.Color
        }).ToList();
    }
}

public class TaskTagDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Color { get; set; } = null!;
} 