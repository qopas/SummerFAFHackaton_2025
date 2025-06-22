using FluentValidation;
using Ediki.Application.Commands.Auth.UpdateUser;

namespace Ediki.Application.Validators.Auth;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required");

        RuleFor(x => x.FirstName)
            .MaximumLength(50).WithMessage("First name cannot exceed 50 characters")
            .When(x => !string.IsNullOrEmpty(x.FirstName));

        RuleFor(x => x.LastName)
            .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters")
            .When(x => !string.IsNullOrEmpty(x.LastName));

        RuleFor(x => x.PreferredRole)
            .IsInEnum().WithMessage("Invalid preferred role specified")
            .When(x => x.PreferredRole.HasValue);

        RuleFor(x => x.University)
            .MaximumLength(100).WithMessage("University name cannot exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.University));

        RuleFor(x => x.GraduationYear)
            .GreaterThanOrEqualTo(1900).WithMessage("Graduation year must be valid")
            .LessThanOrEqualTo(DateTime.Now.Year + 10).WithMessage("Graduation year cannot be too far in the future")
            .When(x => x.GraduationYear.HasValue);

        RuleFor(x => x.Location)
            .MaximumLength(100).WithMessage("Location cannot exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.Location));

        RuleFor(x => x.Skills)
            .Must(skills => skills == null || skills.Count <= 20)
            .WithMessage("Cannot have more than 20 skills")
            .When(x => x.Skills != null);

        // Social Links validation
        RuleFor(x => x.SocialLinks!.Github)
            .Must(BeValidUrl).WithMessage("Invalid GitHub URL")
            .When(x => x.SocialLinks != null && !string.IsNullOrEmpty(x.SocialLinks.Github));

        RuleFor(x => x.SocialLinks!.Linkedin)
            .Must(BeValidUrl).WithMessage("Invalid LinkedIn URL")
            .When(x => x.SocialLinks != null && !string.IsNullOrEmpty(x.SocialLinks.Linkedin));

        RuleFor(x => x.SocialLinks!.Portfolio)
            .Must(BeValidUrl).WithMessage("Invalid Portfolio URL")
            .When(x => x.SocialLinks != null && !string.IsNullOrEmpty(x.SocialLinks.Portfolio));
    }

    private static bool BeValidUrl(string? url)
    {
        if (string.IsNullOrEmpty(url)) return true;
        return Uri.TryCreate(url, UriKind.Absolute, out var result) && 
               (result.Scheme == Uri.UriSchemeHttp || result.Scheme == Uri.UriSchemeHttps);
    }
} 