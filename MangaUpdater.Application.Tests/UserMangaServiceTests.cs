﻿using AutoMapper;
using MangaUpdater.Application.DTOs;
using MangaUpdater.Application.Mappings;
using MangaUpdater.Application.Services;
using MangaUpdater.Domain.Entities;
using MangaUpdater.Domain.Exceptions;
using MangaUpdater.Domain.Interfaces;

namespace MangaUpdater.Application.Tests;

public class UserMangaServiceTests
{
    private readonly Mock<IUserMangaRepository> _userMangaRepository;
    private readonly UserMangaService _service;

    public UserMangaServiceTests()
    {
        var profile = new MappingProfile();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(profile));
        var mapper = new Mapper(configuration);

        _userMangaRepository = new Mock<IUserMangaRepository>();
        _service = new UserMangaService(mapper, _userMangaRepository.Object);
    }

    [Fact]
    public async Task GetMangasByUserId_Should_Return_MangaUserDtos()
    {
        // Arrange
        var sampleUserMangas = new List<UserManga>
        {
            new()
            {
                Id = 1,
                MangaId = 1,
                SourceId = 1,
                UserId = "1",
                CurrentChapterId = 1,
                Manga = new Manga
                {
                    Id = 1, CoverUrl = "url1", Synopsis = "", Type = "Manga", MyAnimeListId = 1,
                    MangaTitles = new List<MangaTitle> { new() { Id = 1, MangaId = 1, Name = "Manga1" } }
                }
            },
            new()
            {
                Id = 2,
                MangaId = 2,
                SourceId = 1,
                UserId = "1",
                CurrentChapterId = 2,
                Manga = new Manga
                {
                    Id = 2, CoverUrl = "url2", Synopsis = "", Type = "Manga", MyAnimeListId = 2,
                    MangaTitles = new List<MangaTitle> { new() { Id = 2, MangaId = 2, Name = "Manga2" } }
                }
            },
            new()
            {
                Id = 3,
                MangaId = 3,
                SourceId = 1,
                UserId = "1",
                CurrentChapterId = 3,
                Manga = new Manga
                {
                    Id = 3, CoverUrl = "url3", Synopsis = "", Type = "Manga", MyAnimeListId = 3,
                    MangaTitles = new List<MangaTitle> { new() { Id = 3, MangaId = 3, Name = "Manga3" } }
                }
            },
        };
        var expectedDto = new List<MangaUserDto>
        {
            new(1, "url1", "Manga1"),
            new(2, "url2", "Manga2"),
            new(3, "url3", "Manga3")
        };

        _userMangaRepository
            .Setup(repo => repo.GetAllByUserIdAsync(It.IsAny<string>()))
            .ReturnsAsync(sampleUserMangas);

        // Act
        var result = await _service.GetMangasByUserId(It.IsAny<string>());

        // Assert
        _userMangaRepository.Verify(repo => repo.GetAllByUserIdAsync(It.IsAny<string>()), Times.Once);
        result.Should().BeEquivalentTo(expectedDto);
    }

    [Fact]
    public async Task GetByMangaIdUserIdAndSourceId_Should_Return_UserManga()
    {
        // Arrange
        var sampleUserManga = new UserManga { Id = 1, MangaId = 1, SourceId = 1, UserId = "1", CurrentChapterId = 1 };

        _userMangaRepository
            .Setup(repo =>
                repo.GetByMangaIdUserIdAndSourceIdAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync(sampleUserManga);

        // Act
        var result = await _service.GetByMangaIdUserIdAndSourceId(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>());

        // Assert
        _userMangaRepository.Verify(
            repo => repo.GetByMangaIdUserIdAndSourceIdAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>()),
            Times.Once);
        result.Should().BeEquivalentTo(sampleUserManga);
    }

    [Fact]
    public async Task GetByMangaIdUserIdAndSourceId_Should_Throw_Exception_When_UserManga_Is_Null()
    {
        // Arrange
        _userMangaRepository
            .Setup(repo =>
                repo.GetByMangaIdUserIdAndSourceIdAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync(() => null);

        // Act and Assert
        await Assert.ThrowsAsync<ValidationException>(() =>
            _service.GetByMangaIdUserIdAndSourceId(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>()));
    }

    [Fact]
    public async Task Update_Should_Update_UserManga_And_SaveChanges()
    {
        // Act
        await _service.Update(It.IsAny<UserManga>());

        // Assert
        _userMangaRepository.Verify(repo => repo.Update(It.IsAny<UserManga>()), Times.Once);
        _userMangaRepository.Verify(repo => repo.SaveAsync(), Times.Once);
    }
}