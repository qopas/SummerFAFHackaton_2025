using Ediki.Domain.Common;
using Ediki.Domain.Enums;

namespace Ediki.Domain.ValueObjects;

public class ProjectResource : ValueObject
{
    public string Title { get; private set; }
    public string Url { get; private set; }
    public ResourceType Type { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private ProjectResource() { }

    public ProjectResource(string title, string url, ResourceType type)
    {
        Title = title;
        Url = url;
        Type = type;
        CreatedAt = DateTime.UtcNow;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Title;
        yield return Url;
        yield return Type;
        yield return CreatedAt;
    }
} 