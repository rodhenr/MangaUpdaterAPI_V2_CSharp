using System.ComponentModel;
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
        await _mediator.Send(command);
    }

    [DisplayName("{0}")]
    public async Task Send<TRequest>(string jobName, TRequest command)
    {
        await _mediator.Send(command);
    }
}