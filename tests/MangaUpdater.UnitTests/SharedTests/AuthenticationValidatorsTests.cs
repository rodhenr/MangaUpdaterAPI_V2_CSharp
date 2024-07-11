using FluentValidation.TestHelper;
using MangaUpdater.Features.Auth.Commands;
using MangaUpdater.Features.Auth.Queries;
using MangaUpdater.Features.User.Commands;
using MangaUpdater.Shared;

namespace MangaUpdater.UnitTests.SharedTests;

public class AuthenticationValidatorsTests
{
    private readonly AuthenticateUserValidator _authenticateValidator;
    private readonly RegisterUserValidator _registerUserValidator;
    private readonly UpdateUserEmailValidator _updateUserEmailvalidator;
    private readonly UpdateUserPasswordValidator _updateUserPasswordValidator;
    
    public AuthenticationValidatorsTests()
    {
         _authenticateValidator = new AuthenticateUserValidator();
         _registerUserValidator = new RegisterUserValidator();
         _updateUserEmailvalidator = new UpdateUserEmailValidator();
         _updateUserPasswordValidator = new UpdateUserPasswordValidator();
    }

    [Fact]
    public void AuthenticateUserValidator_Should_HaveError_When_EmailIsInvalid()
    {
        // Arrange
        var model = new AuthenticateUserQuery("invalid-email", "ValidPassword123!");
        
        // Act
        var result = _authenticateValidator.TestValidate(model);
        
        // Assert
        result.ShouldHaveValidationErrorFor(user => user.Email).WithErrorMessage("Invalid email.");
    }

    [Fact]
    public void AuthenticateUserValidator_Should_NotHaveError_When_EmailIsValid()
    {
        // Arrange
        var model = new AuthenticateUserQuery("valid.email@example.com", "ValidPassword123!");
        
        // Act
        var result = _authenticateValidator.TestValidate(model);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(user => user.Email);
    }

    [Fact]
    public void AuthenticateUserValidator_Should_HaveError_When_PasswordIsEmpty()
    {
        // Arrange
        var model = new AuthenticateUserQuery("valid.email@example.com", "");
        
        // Act
        var result = _authenticateValidator.TestValidate(model);
        
        // Assert
        result.ShouldHaveValidationErrorFor(user => user.Password).WithErrorMessage("Invalid password.");
    }

    [Fact]
    public void AuthenticateUserValidator_Should_NotHaveError_When_PasswordIsNotEmpty()
    {
        // Arrange
        var model = new AuthenticateUserQuery("valid.email@example.com", "ValidPassword123!");
        
        // Act
        var result = _authenticateValidator.TestValidate(model);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(user => user.Password);
    }
    
    [Fact]
    public void RegisterUserValidator_Should_HaveError_When_UserNameIsEmpty()
    {
        // Arrange
        var model = new RegisterUserCommand("", "", "", "");
        
        // Act
        var result = _registerUserValidator.TestValidate(model);
        
        // Assert
        result.ShouldHaveValidationErrorFor(user => user.UserName).WithErrorMessage("The User Name field cannot be empty.");
    }

    [Fact]
    public void RegisterUserValidator_Should_HaveError_When_UserNameIsTooShort()
    {
        // Arrange
        var model = new RegisterUserCommand("a", "", "", "");
        
        // Act
        var result = _registerUserValidator.TestValidate(model);
        
        // Assert
        result.ShouldHaveValidationErrorFor(user => user.UserName).WithErrorMessage("The User Name field must be between 3 and 20 characters.");
    }

    [Fact]
    public void RegisterUserValidator_Should_HaveError_When_UserNameIsTooLong()
    {
        // Arrange
        var model = new RegisterUserCommand(new string('a', 21), "", "", "");
        
        // Act
        var result = _registerUserValidator.TestValidate(model);
        
        // Assert
        result.ShouldHaveValidationErrorFor(user => user.UserName).WithErrorMessage("The User Name field must be between 3 and 20 characters.");
    }

    [Fact]
    public void RegisterUserValidator_Should_HaveError_When_UserNameContainsSpaces()
    {
        // Arrange
        var model = new RegisterUserCommand("User Name", "", "", "");
        
        // Act
        var result = _registerUserValidator.TestValidate(model);
        
        // Assert
        result.ShouldHaveValidationErrorFor(user => user.UserName).WithErrorMessage("The User Name field cannot contain spaces.");
    }

    [Fact]
    public void RegisterUserValidator_Should_NotHaveError_When_UserNameIsValid()
    {
        // Arrange
        var model = new RegisterUserCommand("ValidUserName", "", "", "");
        
        // Act
        var result = _registerUserValidator.TestValidate(model);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(user => user.UserName);
    }

    [Fact]
    public void RegisterUserValidator_Should_HaveError_When_EmailIsInvalid()
    {
        // Arrange
        var model = new RegisterUserCommand("", "invalid-email", "", "");
        
        // Act
        var result = _registerUserValidator.TestValidate(model);
        
        // Assert
        result.ShouldHaveValidationErrorFor(user => user.Email).WithErrorMessage("Invalid email.");
    }

    [Fact]
    public void RegisterUserValidator_Should_NotHaveError_When_EmailIsValid()
    {
        // Arrange
        var model = new RegisterUserCommand("",  "valid.email@example.com", "", "");
        
        // Act
        var result = _registerUserValidator.TestValidate(model);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(user => user.Email);
    }

    [Fact]
    public void RegisterUserValidator_Should_HaveError_When_PasswordIsEmpty()
    {
        // Arrange
        var model = new RegisterUserCommand("",  "", "", "");
        
        // Act
        var result = _registerUserValidator.TestValidate(model);
        
        // Assert
        result.ShouldHaveValidationErrorFor(user => user.Password).WithErrorMessage("Invalid password.");
    }

    [Fact]
    public void RegisterUserValidator_Should_HaveError_When_PasswordIsTooShort()
    {
        // Arrange
        var model = new RegisterUserCommand("",  "", "short1A", "");
        
        // Act
        var result = _registerUserValidator.TestValidate(model);
        
        // Assert
        result.ShouldHaveValidationErrorFor(user => user.Password).WithErrorMessage("Your password length must be at least 8.");
    }

    [Fact]
    public void RegisterUserValidator_Should_HaveError_When_PasswordIsTooLong()
    {
        // Arrange
        var model = new RegisterUserCommand("",  "", new string('a', 33) + "A1!", "");
            
        // Act
        var result = _registerUserValidator.TestValidate(model);
        
        // Assert
        result.ShouldHaveValidationErrorFor(user => user.Password).WithErrorMessage("Your password length must not exceed 32.");
    }

    [Fact]
    public void RegisterUserValidator_Should_HaveError_When_PasswordDoesNotContainUppercase()
    {
        // Arrange
        var model = new RegisterUserCommand("",  "", "lowercase1", "");
        
        // Act
        var result = _registerUserValidator.TestValidate(model);
        
        // Assert
        result.ShouldHaveValidationErrorFor(user => user.Password).WithErrorMessage("Your password must contain at least one uppercase letter.");
    }

    [Fact]
    public void RegisterUserValidator_Should_HaveError_When_PasswordDoesNotContainLowercase()
    {
        // Arrange
        var model = new RegisterUserCommand("",  "", "UPPERCASE1", "");
        
        // Act
        var result = _registerUserValidator.TestValidate(model);
        
        // Assert
        result.ShouldHaveValidationErrorFor(user => user.Password).WithErrorMessage("Your password must contain at least one lowercase letter.");
    }

    [Fact]
    public void RegisterUserValidator_Should_HaveError_When_PasswordDoesNotContainNumber()
    {
        // Arrange
        var model = new RegisterUserCommand("", "", "NoNumber!", "");
        
        // Act
        var result = _registerUserValidator.TestValidate(model);
        
        // Assert
        result.ShouldHaveValidationErrorFor(user => user.Password).WithErrorMessage("Your password must contain at least one number.");
    }

    [Fact]
    public void RegisterUserValidator_Should_HaveError_When_PasswordDoesNotContainSpecialCharacter()
    {
        // Arrange
        var model = new RegisterUserCommand("", "", "NoSpecial1", "");
        
        // Act
        var result = _registerUserValidator.TestValidate(model);
        
        // Assert
        result.ShouldHaveValidationErrorFor(user => user.Password).WithErrorMessage("Your password must contain at least one (!? *.).");
    }

    [Fact]
    public void RegisterUserValidator_Should_NotHaveError_When_PasswordIsValid()
    {
        // Arrange
        var model = new RegisterUserCommand("", "", "Valid1Password!", "");
        
        // Act
        var result = _registerUserValidator.TestValidate(model);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(user => user.Password);
    }

    [Fact]
    public void RegisterUserValidator_Should_HaveError_When_ConfirmationPasswordDoesNotMatch()
    {
        // Arrange
        var model = new RegisterUserCommand("", "", "Valid1Password!", "DifferentPassword!");
        
        // Act
        var result = _registerUserValidator.TestValidate(model);
        
        // Assert
        result.ShouldHaveValidationErrorFor(user => user.ConfirmationPassword).WithErrorMessage("Passwords don't match");
    }

    [Fact]
    public void RegisterUserValidator_Should_NotHaveError_When_ConfirmationPasswordMatches()
    {
        // Arrange
        var model = new RegisterUserCommand("", "", "Valid1Password!", "Valid1Password!");
        
        // Act
        var result = _registerUserValidator.TestValidate(model);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(user => user.ConfirmationPassword);
    }
    
    [Fact]
    public void UpdateUserEmailValidator_Should_HaveErrors_When_EmailIsInvalid()
    {
        // Arrange
        var model = new UpdateUserEmailCommand("invalid-email", "", "");

        // Act
        var result = _updateUserEmailvalidator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(user => user.Email)
            .WithErrorMessage("Invalid email.");
    }

    [Fact]
    public void UpdateUserEmailValidator_Should_HaveErrors_When_PasswordIsInvalid()
    {
        // Arrange
        var model = new UpdateUserEmailCommand("", "", "");

        // Act
        var result = _updateUserEmailvalidator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(user => user.Password)
            .WithErrorMessage("Invalid password.");
    }

    [Fact]
    public void UpdateUserEmailValidator_Should_HaveErrors_When_PasswordLengthIsInvalid()
    {
        // Arrange
        var shortPassword = new UpdateUserEmailCommand("", "short", "");
        var longPassword = new UpdateUserEmailCommand("", new string('a', 33), "");

        // Act
        var shortPasswordResult = _updateUserEmailvalidator.TestValidate(shortPassword);
        var longPasswordResult = _updateUserEmailvalidator.TestValidate(longPassword);

        // Assert
        shortPasswordResult.ShouldHaveValidationErrorFor(user => user.Password)
            .WithErrorMessage("Your password length must be at least 8.");
        longPasswordResult.ShouldHaveValidationErrorFor(user => user.Password)
            .WithErrorMessage("Your password length must not exceed 32.");
    }

    [Fact]
    public void UpdateUserEmailValidator_Should_HaveErrors_When_ConfirmationPasswordDoesNotMatch()
    {
        // Arrange
        var model = new UpdateUserEmailCommand("", "ValidPassword123", "DifferentPassword123");

        // Act
        var result = _updateUserEmailvalidator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(user => user.ConfirmationPassword)
            .WithErrorMessage("Passwords don't match");
    }

    [Fact]
    public void UpdateUserEmailValidator_Should_NotHaveErrors_When_ModelIsValid()
    {
        // Arrange
        var model = new UpdateUserEmailCommand("valid.email@example.com", "ValidPassword123", "ValidPassword123");

        // Act
        var result = _updateUserEmailvalidator.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(user => user.Email);
        result.ShouldNotHaveValidationErrorFor(user => user.Password);
        result.ShouldNotHaveValidationErrorFor(user => user.ConfirmationPassword);
    }
    
    [Fact]
    public void Should_HaveErrors_When_OldPasswordIsInvalid()
    {
        // Arrange
        var model = new UpdateUserPasswordCommand("", "");

        // Act
        var result = _updateUserPasswordValidator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(user => user.OldPassword)
            .WithErrorMessage("Invalid password.");
    }

    [Fact]
    public void Should_HaveErrors_When_PasswordIsInvalid()
    {
        // Arrange
        var model = new UpdateUserPasswordCommand("", "");

        // Act
        var result = _updateUserPasswordValidator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(user => user.Password)
            .WithErrorMessage("Invalid password.");
    }

    [Fact]
    public void Should_HaveErrors_When_PasswordLengthIsInvalid()
    {
        // Arrange
        var shortPassword = new UpdateUserPasswordCommand("short", "");
        var longPassword = new UpdateUserPasswordCommand(new string('a', 33), "");

        // Act
        var shortPasswordResult = _updateUserPasswordValidator.TestValidate(shortPassword);
        var longPasswordResult = _updateUserPasswordValidator.TestValidate(longPassword);

        // Assert
        shortPasswordResult.ShouldHaveValidationErrorFor(user => user.Password)
            .WithErrorMessage("Your password length must be at least 8.");
        longPasswordResult.ShouldHaveValidationErrorFor(user => user.Password)
            .WithErrorMessage("Your password length must not exceed 32.");
    }

    [Fact]
    public void Should_HaveErrors_When_PasswordComplexityIsInvalid()
    {
        // Arrange
        var noUpperCase = new UpdateUserPasswordCommand("validpassword123!", "");
        var noLowerCase = new UpdateUserPasswordCommand("VALIDPASSWORD123!", "");
        var noNumber = new UpdateUserPasswordCommand("ValidPassword!", "");
        var noSpecialChar = new UpdateUserPasswordCommand("ValidPassword123", "");

        // Act
        var noUpperCaseResult = _updateUserPasswordValidator.TestValidate(noUpperCase);
        var noLowerCaseResult = _updateUserPasswordValidator.TestValidate(noLowerCase);
        var noNumberResult = _updateUserPasswordValidator.TestValidate(noNumber);
        var noSpecialCharResult = _updateUserPasswordValidator.TestValidate(noSpecialChar);

        // Assert
        noUpperCaseResult.ShouldHaveValidationErrorFor(user => user.Password)
            .WithErrorMessage("Your password must contain at least one uppercase letter.");
        noLowerCaseResult.ShouldHaveValidationErrorFor(user => user.Password)
            .WithErrorMessage("Your password must contain at least one lowercase letter.");
        noNumberResult.ShouldHaveValidationErrorFor(user => user.Password)
            .WithErrorMessage("Your password must contain at least one number.");
        noSpecialCharResult.ShouldHaveValidationErrorFor(user => user.Password)
            .WithErrorMessage("Your password must contain at least one (!? *.).");
    }

    [Fact]
    public void Should_NotHaveErrors_When_ModelIsValid()
    {
        // Arrange
        var model = new UpdateUserPasswordCommand("ValidOldPassword123!", "ValidOldPassword123!");

        // Act
        var result = _updateUserPasswordValidator.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(user => user.OldPassword);
        result.ShouldNotHaveValidationErrorFor(user => user.Password);
    }
}