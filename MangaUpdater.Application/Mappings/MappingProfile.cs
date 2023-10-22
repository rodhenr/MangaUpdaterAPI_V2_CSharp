using AutoMapper;
using MangaUpdater.Application.DTOs;
using MangaUpdater.Application.Models.External.MyAnimeList;
using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Manga, MangaDto>()
            .ForMember(dest => dest.MangaId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.MangaTitles!.First().Name))
            .ForMember(dest => dest.AlternativeName,
                opt => opt.MapFrom(src =>
                    src.MangaTitles!.Count() > 1 ? src.MangaTitles!.ElementAt(1).Name : src.MangaTitles!.First().Name))
            .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.MangaAuthors!.First().Name))
            .ForMember(dest => dest.IsUserFollowing,
                opt => opt.MapFrom(src => src.UserMangas != null && src.UserMangas.Any()))
            .ForMember(dest => dest.Sources,
                opt => opt.MapFrom(src =>
                    src.MangaSources!.Select(ms => new SourceDto(ms.Source!.Id, ms.Source.Name, ms.Source.BaseUrl))))
            .ForMember(dest => dest.Genres, opt => opt.MapFrom(src =>
                src.MangaGenres != null && src.MangaGenres.Any()
                    ? src.MangaGenres.Select(mg => mg.Genre!.Name)
                    : Enumerable.Empty<string>()
            ))
            .ForMember(dest => dest.Chapters, opt => opt.MapFrom((src, _, _, _) =>
            {
                if (src.Chapters is null || !src.Chapters.Any())
                    return Enumerable.Empty<ChapterDto>();

                var userSourceChapterList =
                    src.UserMangas?
                        .Where(um => um.UserChapter is not null)
                        .Select(um => new { um.UserChapter!.SourceId, um.UserChapter!.ChapterId })
                        .ToList();

                return src.Chapters
                    .Select(ch =>
                    {
                        return new ChapterDto
                        {
                            ChapterId = ch.Id,
                            SourceId = ch.Source!.Id,
                            SourceName = ch.Source.Name,
                            Date = ch.Date,
                            Number = ch.Number,
                            Read = userSourceChapterList is not null &&
                                   userSourceChapterList.Any(chapterList => chapterList.SourceId == ch.SourceId) &&
                                   ch.Id <=
                                   userSourceChapterList.First(chapterListRead =>
                                           chapterListRead.SourceId == ch.SourceId)
                                       .ChapterId
                        };
                    });
            }));

        CreateMap<UserMangaGroupByMangaDto, MangaUserLoggedDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Manga.Id))
            .ForMember(dest => dest.CoverUrl, opt => opt.MapFrom(src => src.Manga.CoverUrl))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Manga!.MangaTitles!.First().Name))
            .ForMember(dest => dest.Chapters, opt => opt.MapFrom((src, _, _, _) =>
            {
                return src.Manga.Chapters?
                    .Select(ch => new ChapterDto
                    {
                        ChapterId = ch.Id,
                        SourceId = ch.SourceId,
                        SourceName = ch.Source!.Name,
                        Date = ch.Date,
                        Number = ch.Number,
                        Read = src.SourcesWithLastChapterRead.Any(b => b.SourceId == ch.SourceId) && ch.Id <=
                            src.SourcesWithLastChapterRead.First(c => c.SourceId == ch.SourceId).LastChapterRead
                    });
            }));

        CreateMap<MyAnimeListApiResponse, Manga>()
            .ForMember(dest => dest.CoverUrl, opt => opt.MapFrom(src => src.Images.JPG.LargeImageUrl))
            .ForMember(dest => dest.MyAnimeListId, opt => opt.MapFrom(src => src.MalId));

        CreateMap<Manga, MangaUserDto>()
            .ForCtorParam("MangaId", opt => opt.MapFrom(src => src.Id))
            .ForCtorParam("CoverUrl", opt => opt.MapFrom(src => src.CoverUrl))
            .ForCtorParam("MangaName", opt => opt.MapFrom(src => src.MangaTitles!.First().Name));
    }
}