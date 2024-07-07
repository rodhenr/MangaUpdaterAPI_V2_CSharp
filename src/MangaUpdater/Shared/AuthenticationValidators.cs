using FluentValidation;
using MangaUpdater.Features.Auth.Commands;
using MangaUpdater.Features.Auth.Queries;

namespace MangaUpdater.Shared;

public class AuthenticateUserValidator : AbstractValidator<AuthenticateUserQuery>
{
    public AuthenticateUserValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Invalid email.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Invalid password.");
    }
}

public class RegisterUserValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserValidator()
    {
        RuleFor(x => x.UserName)            
            .NotEmpty().WithMessage("The {PropertyName} field cannot be empty.")
            .Length(3, 20).WithMessage("The {PropertyName} field must be between {MinLength} and {MaxLength} characters.")
            .Must(username => !username.Any(char.IsWhiteSpace)).WithMessage("The {PropertyName} field cannot contain spaces.");
                    
        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Invalid email.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Invalid password.")
            .MinimumLength(8).WithMessage("Your password length must be at least 8.")
            .MaximumLength(32).WithMessage("Your password length must not exceed 32.")
            .Matches(@"[A-Z]+").WithMessage("Your password must contain at least one uppercase letter.")
            .Matches(@"[a-z]+").WithMessage("Your password must contain at least one lowercase letter.")
            .Matches(@"[0-9]+").WithMessage("Your password must contain at least one number.")
            .Matches(@"[\!\?\*\.]+").WithMessage("Your password must contain at least one (!? *.).");

        RuleFor(x => x.ConfirmationPassword)
            .Equal(x => x.Password).WithMessage("Passwords don't match");
    }
}

public class UpdateUserEmailValidator : AbstractValidator<UpdateUserEmailCommand>
{
    public UpdateUserEmailValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Invalid email.");
        
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Invalid password.")
            .MinimumLength(8).WithMessage("Your password length must be at least 8.")
            .MaximumLength(32).WithMessage("Your password length must not exceed 32.");
        
        RuleFor(x => x.ConfirmationPassword)
            .Equal(x => x.Password).WithMessage("Passwords don't match");
    }
}

public class UpdateUserPasswordValidator : AbstractValidator<UpdateUserPasswordCommand>
{
    public UpdateUserPasswordValidator()
    {
        RuleFor(x => x.OldPassword)
            .NotEmpty().WithMessage("Invalid password.");
        
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Invalid password.")
            .MinimumLength(8).WithMessage("Your password length must be at least 8.")
            .MaximumLength(32).WithMessage("Your password length must not exceed 32.")
            .Matches(@"[A-Z]+").WithMessage("Your password must contain at least one uppercase letter.")
            .Matches(@"[a-z]+").WithMessage("Your password must contain at least one lowercase letter.")
            .Matches(@"[0-9]+").WithMessage("Your password must contain at least one number.")
            .Matches(@"[\!\?\*\.]+").WithMessage("Your password must contain at least one (!? *.).");
    }
}