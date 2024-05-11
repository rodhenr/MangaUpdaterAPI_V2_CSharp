using Hangfire;
using MangaUpdater.Core.Services;
using MediatR;

namespace MangaUpdater.Core.Common.Extensions;

public static class MediatorExtensions
{
    public static string Enqueue(IRequest request)
    {
        var client = new BackgroundJobClient();
        return client.Enqueue<MediatorHangfireBridge>(bridge => bridge.Send(request));
    }
    
    public static string Enqueue(this IMediator mediator,IRequest request)
    {
        var client = new BackgroundJobClient();
        return client.Enqueue<MediatorHangfireBridge>(bridge => bridge.Send(request));
    }
}