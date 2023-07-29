﻿using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Application.Interfaces;

public interface IUserService
{
    Task AddUser(User user);
    Task<User?> GetById(int id);
}