using Hangfire;
using MangaUpdater.Core.Services;
using MediatR;

namespace MangaUpdater.Core.Common.Extensions;

public static class MediatorExtensions
{
    public static void Enqueue<TRequest>(this IMediator mediator, string jobName, TRequest request)
    {
        var client = new BackgroundJobClient();
        client.Enqueue<MediatorHangfireBridge>(bridge => bridge.Send(jobName, request));
    }

    public static void Enqueue(this IMediator mediator,IRequest request)
    {
        var client = new BackgroundJobClient();
        client.Enqueue<MediatorHangfireBridge>(bridge => bridge.Send(request));
    }
}