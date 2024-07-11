using MediatR;

namespace MangaUpdater.Services.Hangfire;

public class MediatorHangfireBridge
{
    private readonly ISender _mediator;

    public MediatorHangfireBridge(ISender mediator)
    {
        _mediator = mediator;
    }

    public async Task Send(IRequest command)
    {
        await _mediator.Send(command);
    }
}