using MangaUpdater.Core.Common.Exceptions;
using MangaUpdater.Core.Services;
using MangaUpdater.Data;
using MangaUpdater.Data.Entities.Models;
using MangaUpdater.Data.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using InvalidOperationException = System.InvalidOperationException;

namespace MangaUpdater.Core.Features.Users;

public record UpdateFollowedSourcesCommand(int MangaId, List<int> SourceIds) : IRequest;

public sealed class UpdateFollowedSourcesHandler : IRequestHandler<UpdateFollowedSourcesCommand>
{
    private readonly AppDbContextIdentity _context;
    private readonly CurrentUserAccessor _currentUserAccessor;
    
    public UpdateFollowedSourcesHandler(AppDbContextIdentity context, CurrentUserAccessor currentUserAccessor)
    {
        _context = context;
        _currentUserAccessor = currentUserAccessor;
    }

    public async Task Handle(UpdateFollowedSourcesCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserAccessor.UserId;

        await VerifySources(request, cancellationToken);
        var userManga = await CreateAndGetUserManga(request.MangaId, userId, cancellationToken);
        
        var userSources = request.SourceIds
            .Select(x => new UserChapter
            {
                UserMangaId = userManga.Id, 
                ChapterId = null, 
                SourceId = x
            })
            .ToList();
        
        _context.UserChapters.AddRange(userSources);
        await _context.SaveChangesAsync(cancellationToken);
    }

    private async Task VerifySources(UpdateFollowedSourcesCommand request, CancellationToken cancellationToken)
    {
        var mangaSources = await _context.MangaSources
            .Where(x => x.MangaId == request.MangaId && request.SourceIds.Contains(x.SourceId))
            .Select(x => x.SourceId)
            .ToListAsync(cancellationToken: cancellationToken);
        
        var listsAreEqual = mangaSources
            .OrderBy(x => x)
            .SequenceEqual(request.SourceIds.OrderBy(x => x));

        if (!listsAreEqual) throw new InvalidOperationException("Some of the source IDs do not exist.");
    }

    private async Task<UserManga> CreateAndGetUserManga(int mangaId, string userId, CancellationToken cancellationToken)
    {
        var userManga = await _context.UserMangas.GetByMangaIdAndUserId(mangaId, userId, cancellationToken);
        if (userManga is not null) return userManga;
        
        _context.UserMangas.Add(new UserManga { MangaId = mangaId, UserId = userId! });
        await _context.SaveChangesAsync(cancellationToken);
        
        var newUserManga = await _context.UserMangas.GetByMangaIdAndUserId(mangaId, userId, cancellationToken);
        if (newUserManga is null) throw new EntityNotFoundException("UserManga not found.");
        
        return newUserManga;
    }
}