using MediatR;

namespace MangaUpdater.Core.Services;

public class MediatorHangfireBridge
{
    private readonly IMediator _mediator;

    public MediatorHangfireBridge(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Send<TRequest>(TRequest command)
    {
        if (command != null) await _mediator.Send(command);
    }
}