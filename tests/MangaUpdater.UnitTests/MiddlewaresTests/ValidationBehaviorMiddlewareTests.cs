using FluentValidation;
using FluentValidation.Results;
using MangaUpdater.Middlewares;
using MediatR;
using NSubstitute;

namespace MangaUpdater.UnitTests.MiddlewaresTests;

public class FakeRequest;
public class FakeResponse;

public class ValidationBehaviorMiddlewareTests
{
    private readonly RequestHandlerDelegate<FakeResponse> _requestHandlerDelegate;
    
    public ValidationBehaviorMiddlewareTests()
    {
        _requestHandlerDelegate = Substitute.For<RequestHandlerDelegate<FakeResponse>>();
    }
    
    [Fact]
    public async Task Handle_NoValidators_CallsNextDelegate()
    {
        // Arrange
        var validators = Enumerable.Empty<IValidator<FakeRequest>>();
        var middleware = new ValidationBehaviorMiddleware<FakeRequest, FakeResponse>(validators);

        // Act
        await middleware.Handle(new FakeRequest(), _requestHandlerDelegate, CancellationToken.None);

        // Assert
        await _requestHandlerDelegate.Received(1).Invoke();
    }

    [Fact]
    public async Task Handle_ValidatorsWithNoFailures_CallsNextDelegate()
    {
        // Arrange
        var validator = Substitute.For<IValidator<FakeRequest>>();
        
        validator.ValidateAsync(Arg.Any<IValidationContext>(), Arg.Any<CancellationToken>())
                 .Returns(Task.FromResult(new ValidationResult()));
        
        var validators = new List<IValidator<FakeRequest>> { validator };
        var middleware = new ValidationBehaviorMiddleware<FakeRequest, FakeResponse>(validators);

        // Act
        await middleware.Handle(new FakeRequest(), _requestHandlerDelegate, CancellationToken.None);

        // Assert
        await _requestHandlerDelegate.Received(1).Invoke();
    }

    [Fact]
    public async Task Handle_ValidatorsWithFailures_ThrowsValidationException()
    {
        // Arrange
        var validator = Substitute.For<IValidator<FakeRequest>>();
        var failures = new List<ValidationFailure> { new ValidationFailure("Property", "Error") };
        
        validator.ValidateAsync(Arg.Any<IValidationContext>(), Arg.Any<CancellationToken>())
                 .Returns(Task.FromResult(new ValidationResult(failures)));

        var validators = new List<IValidator<FakeRequest>> { validator };
        var middleware = new ValidationBehaviorMiddleware<FakeRequest, FakeResponse>(validators);

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => middleware.Handle(new FakeRequest(), _requestHandlerDelegate, CancellationToken.None));
    }
}