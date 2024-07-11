using MangaUpdater.Services.Hangfire;
using MediatR;
using NSubstitute;

namespace MangaUpdater.UnitTests.ServicesTests;

public record SampleCommand(string commandData) : IRequest;

public class MediatorHangfireBridgeTests
{
    private readonly ISender _sender;
    public MediatorHangfireBridgeTests()
    {
        _sender = Substitute.For<ISender>();
    }
    
    [Fact]
    public async Task Send_ShouldInvokeMediatorSend()
    {
        // Arrange
        var bridge = new MediatorHangfireBridge(_sender);
        var command = new SampleCommand("command");

        // Act
        await bridge.Send(command);

        // Assert
        await _sender.Received(1).Send(command);
    }
}