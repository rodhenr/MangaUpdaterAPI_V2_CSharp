using AutoFixture;
using MangaUpdater.DTOs;
using MangaUpdater.Entities;
using MangaUpdater.Features.Mangas.Queries;
using MangaUpdater.IntegrationTests.Setup;
using MangaUpdater.Mappers;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.IntegrationTests.FeaturesTests.Mangas;

public class GetMangasTests : BaseFixture, IAsyncLifetime
{
    private List<Manga> _mangas = null!;
    private Source _source = null!;
    private List<MangaAuthor> _mangaAuthors = null!;
    private List<MangaTitle> _mangaTitles = null!;
    private List<MangaGenre> _mangaGenres = null!;
    private List<MangaSource> _mangaSources = null!;
    
    public GetMangasTests(IntegrationTestWebAppFactory factory) : base(factory) { }
    
    [Fact]
    public async Task Should_Return_Mangas_OrderBy_Alphabet()
    {
        // Arrange
        const string input = "name";
        const int page = 1;
        const int pageSize = 10;
        const int numberOfPages = 2;
        const OrderByEnum orderBy = OrderByEnum.Alphabet;
        var sourceIds = new List<int>
        {
            _source.Id
        };
        var genreIds = _mangaGenres.Select(x => x.GenreId).ToList();

        var mangaInfoResult = _mangas.ToDtos()
            .OrderBy(x => x.MangaName)
            .Take(pageSize)
            .ToList();
        
        var query = new GetMangasQuery(input, page,pageSize, orderBy, sourceIds, genreIds);
        
        // Act
        var result = await Sender.Send(query);
        
        // Assert
        Assert.Equal(pageSize, result.PageSize);
        Assert.Equal(page, result.CurrentPage);
        Assert.Equal(numberOfPages, result.TotalPages);
        Assert.Equal(mangaInfoResult, result.Mangas.ToList());
    }
    
    public new async Task InitializeAsync() => await SeedDb();
        
    private async Task SeedDb()
    {
        // Manga && MangaAuthors && MangaTitles
        const int numberOfRepetitions = 20;
        
        _mangas = Fixture.CreateMany<Manga>(numberOfRepetitions).ToList();
        await InsertRange(_mangas);
        
        _mangaAuthors = Fixture.CreateMany<MangaAuthor>(numberOfRepetitions).ToList();
        _mangaTitles = Fixture.CreateMany<MangaTitle>(numberOfRepetitions).ToList();

        for (var i = 0; i < 20; i++)
        {
            _mangaAuthors[i].MangaId = _mangas[i].MyAnimeListId;
            _mangaTitles[i].MangaId = _mangas[i].MyAnimeListId;
            _mangaTitles[i].IsMyAnimeListMainTitle = true;
        }
        
        await InsertRange(_mangaAuthors);
        await InsertRange(_mangaTitles);
        
        // Genre && MangaGenre
        var genres = await Db.Genres.ToListAsync();
        
        _mangaGenres = Fixture.CreateMany<MangaGenre>(numberOfRepetitions).ToList();
        
        for (var i = 0; i < numberOfRepetitions; i++)
        {
            _mangaGenres[i].MangaId = _mangas[i].MyAnimeListId;
            _mangaGenres[i].GenreId = genres[i].Id;
        }
        
        await InsertRange(_mangaGenres);
        
        // Source
        _source = Fixture.Create<Source>();
        await Insert(_source);
        
        // MangaSource
        _mangaSources = Fixture.CreateMany<MangaSource>(numberOfRepetitions).ToList();
        
        for (var i = 0; i < numberOfRepetitions; i++)
        {
            _mangaSources[i].MangaId = _mangas[i].MyAnimeListId;
            _mangaSources[i].SourceId = _source.Id;
        }
        
        await InsertRange(_mangaSources);
    }
}