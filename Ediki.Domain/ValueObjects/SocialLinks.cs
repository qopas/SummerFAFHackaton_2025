namespace Ediki.Domain.ValueObjects;

public class SocialLinks
{
    public string? Github { get; set; }
    public string? Linkedin { get; set; }
    public string? Portfolio { get; set; }

    public SocialLinks() { }

    public SocialLinks(string? github, string? linkedin, string? portfolio)
    {
        Github = github;
        Linkedin = linkedin;
        Portfolio = portfolio;
    }
} 