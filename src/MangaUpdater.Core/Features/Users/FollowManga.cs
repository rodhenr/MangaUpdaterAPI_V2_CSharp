﻿using MangaUpdater.Core.Services;
using MangaUpdater.Data;
using MangaUpdater.Data.Entities.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MangaUpdater.Core.Features.Users;

public record FollowMangaCommand([FromRoute] int MangaId) : IRequest;

public sealed class FollowMangaHandler : IRequestHandler<FollowMangaCommand>
{
    private readonly AppDbContextIdentity _context;
    private readonly CurrentUserAccessor _currentUserAccessor;
    
    public FollowMangaHandler(AppDbContextIdentity context, CurrentUserAccessor currentUserAccessor)
    {
        _context = context;
        _currentUserAccessor = currentUserAccessor;
    }

    public async Task Handle(FollowMangaCommand request, CancellationToken cancellationToken)
    {
        var userManga = new UserManga
        {
            UserId = _currentUserAccessor.UserId,
            MangaId = request.MangaId
        };
        
        _context.UserMangas.Add(userManga);
        await _context.SaveChangesAsync(cancellationToken);
    }
}