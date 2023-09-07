using MangaUpdater.Domain.Entities;
using MangaUpdater.Domain.Exceptions;

namespace MangaUpdater.Application.Helpers;

public static class ValidationHelper
{
    public static void ValidateEntity(Entity? entity, string message = "Entity has empty")
    {
        if (entity is null) throw new ValidationException(message);
    }
}