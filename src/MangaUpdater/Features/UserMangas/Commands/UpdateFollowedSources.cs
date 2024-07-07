using MangaUpdater.Exceptions;
using MangaUpdater.Services;
using MangaUpdater.Database;
using MangaUpdater.Entities;
using MangaUpdater.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Features.UserMangas.Commands;

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
        await VerifySources(request, cancellationToken);
        var userManga = await CreateAndGetUserManga(request.MangaId, request.SourceIds, cancellationToken);

        if (request.SourceIds.Count == 0)
        {
            var userChapters = await _context.UserChapters
                .Where(x => x.UserMangaId == userManga.Id)
                .ToListAsync(cancellationToken);
            
            _context.UserChapters.RemoveRange(userChapters);
            await _context.SaveChangesAsync(cancellationToken);

            return;
        }
        
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

        if (!listsAreEqual) throw new NotFoundException("Some of the source IDs do not exist.");
    }

    private async Task<UserManga> CreateAndGetUserManga(int mangaId, List<int> sourceIds, CancellationToken cancellationToken)
    {
        var userManga = await _context.UserMangas.GetByMangaIdAndUserId(mangaId, _currentUserAccessor.UserId, cancellationToken);
        
        if (userManga is not null)
        {
            await VerifyUserSources(userManga.Id, sourceIds, cancellationToken);
            return userManga;
        };
        
        _context.UserMangas.Add(new UserManga { MangaId = mangaId, UserId = _currentUserAccessor.UserId });
        await _context.SaveChangesAsync(cancellationToken);
        
        var newUserManga = await _context.UserMangas.GetByMangaIdAndUserId(mangaId, _currentUserAccessor.UserId, cancellationToken);
        if (newUserManga is null) throw new EntityNotFoundException("UserManga not found.");
        
        return newUserManga;
    }
    
    private async Task VerifyUserSources(int userMangaId, List<int> sourceIds, CancellationToken cancellationToken)
    {
        var userChapters = await _context.UserChapters
            .Where(x => x.UserMangaId == userMangaId && sourceIds.Contains(x.SourceId))
            .ToListAsync(cancellationToken: cancellationToken);

        if (userChapters.Count > 0) throw new BadRequestException("User already following one or more sources from the request.");
    }
}