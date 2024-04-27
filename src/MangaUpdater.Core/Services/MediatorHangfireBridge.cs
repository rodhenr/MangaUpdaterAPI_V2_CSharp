using MediatR;

namespace MangaUpdater.Core.Services;

public class MediatorHangfireBridge
{
    private readonly IMediator _mediator;

    public MediatorHangfireBridge(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Send(IRequest command)
    {
        await _mediator.Send(command);
    }
}