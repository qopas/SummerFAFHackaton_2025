using Ediki.Domain.Common;

namespace Ediki.Domain.ValueObjects;

public class ProjectRewards : ValueObject
{
    public int Xp { get; private set; }
    public bool Certificates { get; private set; }
    public bool Recommendations { get; private set; }

    private ProjectRewards() { }

    public ProjectRewards(int xp = 100, bool certificates = false, bool recommendations = false)
    {
        Xp = xp;
        Certificates = certificates;
        Recommendations = recommendations;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Xp;
        yield return Certificates;
        yield return Recommendations;
    }
} 