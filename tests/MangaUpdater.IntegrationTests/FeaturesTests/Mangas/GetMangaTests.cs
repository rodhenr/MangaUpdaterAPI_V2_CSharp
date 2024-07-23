using AutoFixture;
using MangaUpdater.DTOs;
using MangaUpdater.Entities;
using MangaUpdater.Features.Mangas.Queries;
using MangaUpdater.IntegrationTests.Setup;
using MangaUpdater.Mappers;

namespace MangaUpdater.IntegrationTests.FeaturesTests.Mangas;

public class GetMangaTests : BaseFixture, IAsyncLifetime
{
    private const string UserId = "someUserId";
    private Manga _manga = null!;
    private Source _source = null!;
    private List<Chapter> _chapters = null!;
    private List<MangaAuthor> _mangaAuthors = null!;
    private List<MangaTitle> _mangaTitles = null!;
    private List<MangaGenre> _mangaGenres = null!;
    private List<MangaSource> _mangaSources = null!;
    
    public GetMangaTests(IntegrationTestWebAppFactory factory) : base(factory) { }
    
    [Fact]
    public async Task Should_Return_Manga_When_User_Is_Not_Logged()
    {
        // Arrange
        var query = new GetMangaQuery(_manga.MyAnimeListId);
        
        // Act
        var result = await Sender.Send(query);
        
        // Assert
        Assert.Equal(_manga.MyAnimeListId, result.Id);
        Assert.Equal(_manga.Synopsis, result.Synopsis);
        Assert.Equal(_manga.Type, result.Type);
        Assert.Equal(_manga.CoverUrl, result.CoverUrl);
        Assert.False(result.IsUserFollowing);
        Assert.Equal(_mangaSources.ToDtos().OrderBy(x => x.Id), result.Sources.OrderBy(x => x.Id));
        Assert.Equal(_chapters.ToDtos().OrderBy(x => x.Id), result.Chapters.OrderBy(x => x.Id));
        Assert.Equal(_mangaAuthors.ToDtos().OrderBy(x => x.Id), result.Authors.OrderBy(x => x.Id));
        Assert.Equal(_mangaGenres.ToDtos().OrderBy(x => x.Id), result.Genres.OrderBy(x => x.Id));
        Assert.Equal(_mangaTitles.ToDtos().OrderBy(x => x.Id), result.Titles.OrderBy(x => x.Id));
    }
    
    [Fact]
    public async Task Should_Return_Manga_When_User_Is_Logged()
    {
        // Arrange
        var query = new GetMangaQuery(_manga.MyAnimeListId);
        AddUserIntoHttpContext(UserId);
        
        // Act
        var result = await Sender.Send(query);

        var followedChapters = _chapters.ToDtos().Select(x => x with{IsRead = true, IsUserAllowedToRead = true});
        
        // Assert
        Assert.Equal(_manga.MyAnimeListId, result.Id);
        Assert.Equal(_manga.Synopsis, result.Synopsis);
        Assert.Equal(_manga.Type, result.Type);
        Assert.Equal(_manga.CoverUrl, result.CoverUrl);
        Assert.True(result.IsUserFollowing);
        Assert.Equal(_mangaSources.ToDtos().OrderBy(x => x.Id), result.Sources.OrderBy(x => x.Id));
        Assert.Equal(followedChapters.OrderBy(x => x.Id), result.Chapters.OrderBy(x => x.Id));
        Assert.Equal(_mangaAuthors.ToDtos().OrderBy(x => x.Id), result.Authors.OrderBy(x => x.Id));
        Assert.Equal(_mangaGenres.ToDtos().OrderBy(x => x.Id), result.Genres.OrderBy(x => x.Id));
        Assert.Equal(_mangaTitles.ToDtos().OrderBy(x => x.Id), result.Titles.OrderBy(x => x.Id));
    }
    
    public new async Task InitializeAsync() => await SeedDb();
        
    private async Task SeedDb()
    {
        // Manga && MangaAuthors && MangaTitles
        _manga = Fixture.Create<Manga>();
        await Insert(_manga);

        const int numberOfRepetitions = 3;
        
        _mangaAuthors = Fixture.CreateMany<MangaAuthor>(numberOfRepetitions).ToList();
        _mangaTitles = Fixture.CreateMany<MangaTitle>(numberOfRepetitions).ToList();

        for (var i = 0; i < numberOfRepetitions; i++)
        {
            _mangaAuthors[i].MangaId = _manga.MyAnimeListId;
            _mangaTitles[i].MangaId = _manga.MyAnimeListId;
            _mangaTitles[i].IsMyAnimeListMainTitle = i == numberOfRepetitions - 1;
        }
        
        await InsertRange(_mangaAuthors);
        await InsertRange(_mangaTitles);
        
        // Genre && MangaGenre
        var genres = Fixture.CreateMany<Genre>(numberOfRepetitions).ToList();
        
        for (var i = 0; i < numberOfRepetitions; i++)
        {
            genres[i].Id = i + 1;
        }
        
        await InsertRange(genres);
       
        _mangaGenres = Fixture.CreateMany<MangaGenre>(numberOfRepetitions).ToList();
        
        for (var i = 0; i < numberOfRepetitions; i++)
        {
            _mangaGenres[i].MangaId = _manga.MyAnimeListId;
            _mangaGenres[i].GenreId = genres[i].Id;
        }
        
        await InsertRange(_mangaGenres);
        
        // Source && MangaSource
        _source = Fixture.Create<Source>();
        await Insert(_source);
        
        var mangaSource = Fixture.Create<MangaSource>();
        mangaSource.MangaId = _manga.MyAnimeListId;
        mangaSource.SourceId = _source.Id;
        await Insert(mangaSource);

        _mangaSources = [mangaSource];
        
        // User && UserManga
        var user = await CreateUser(UserId, true);

        var userManga = Fixture.Create<UserManga>();
        userManga.MangaId = _manga.MyAnimeListId;
        userManga.UserId = user.Id;
        
        await Insert(userManga);
        
        // Chapter && UserChapter
        _chapters = Fixture.CreateMany<Chapter>(numberOfRepetitions).ToList();

        for(var i = 0; i < numberOfRepetitions; i++)
        {
            _chapters[i].MangaId = _manga.MyAnimeListId;
            _chapters[i].SourceId = _source.Id;
            _chapters[i].Number = i.ToString();
        }
        await InsertRange(_chapters);

        var userChapter = Fixture.Create<UserChapter>();
        userChapter.ChapterId = _chapters.MaxBy(x => x.Number)?.Id ?? 2;
        userChapter.UserMangaId = userManga.Id;
        userChapter.SourceId = _source.Id;
        await Insert(userChapter);
    }
}