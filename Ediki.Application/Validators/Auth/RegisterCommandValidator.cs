using FluentValidation;
using Ediki.Application.Commands.Auth.Register;
using Ediki.Domain.Enums;

namespace Ediki.Application.Validators.Auth;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)").WithMessage("Password must contain at least one lowercase letter, one uppercase letter, and one digit");

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage("Confirm password is required")
            .Equal(x => x.Password).WithMessage("Passwords do not match");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(50).WithMessage("First name cannot exceed 50 characters");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters");

        RuleFor(x => x.Role)
            .Must(role => RoleNames.GetAllRoles().Contains(role))
            .WithMessage("Invalid role specified");

        // New field validations
        RuleFor(x => x.PreferredRole)
            .IsInEnum().WithMessage("Invalid preferred role specified");

        RuleFor(x => x.Xp)
            .GreaterThanOrEqualTo(0).WithMessage("XP cannot be negative");

        RuleFor(x => x.Level)
            .GreaterThanOrEqualTo(1).WithMessage("Level must be at least 1");

        RuleFor(x => x.CompletedProjects)
            .GreaterThanOrEqualTo(0).WithMessage("Completed projects cannot be negative");

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

        RuleFor(x => x.Badges)
            .Must(badges => badges == null || badges.Count <= 50)
            .WithMessage("Cannot have more than 50 badges")
            .When(x => x.Badges != null);

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
