using AutoMapper;
using MangaUpdater.Application.DTOs;
using MangaUpdater.Application.Models;
using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Manga, MangaDto>()
            .ForMember(dest => dest.CoverUrl, opt => opt.MapFrom(src => src.CoverUrl))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.AlternativeName, opt => opt.MapFrom(src => src.AlternativeName))
            .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author))
            .ForMember(dest => dest.Synopsis, opt => opt.MapFrom(src => src.Synopsis))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
            .ForMember(dest => dest.MyAnimeListId, opt => opt.MapFrom(src => src.MyAnimeListId))
            .ForMember(dest => dest.IsUserFollowing, opt => opt.MapFrom(src => src.UserMangas!.Any()))
            .ForMember(dest => dest.Sources,
                opt => opt.MapFrom(src =>
                    src.MangaSources!.Select(a => new SourceDto(a.Source!.Id, a.Source.Name, a.Source.BaseUrl))))
            .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.MangaGenres!.Select(a => a.Genre!.Name)))
            .ForMember(dest => dest.Chapters, opt => opt.MapFrom((src, _, _, _) =>
            {
                if (src.UserMangas is null || src.Chapters is null)
                {
                    return null;
                }

                var userSourceChapterList = src.UserMangas.Select(a => new { a.SourceId, a.CurrentChapterId }).ToList();

                return src.Chapters
                    .Select(ch =>
                    {
                        return new ChapterDto(ch.Id, ch.Source!.Id, ch.Source.Name, ch.Date, ch.Number,
                            userSourceChapterList.Any(chapterList => chapterList.SourceId == ch.SourceId) && ch.Id <=
                            userSourceChapterList.First(chapterList => chapterList.SourceId == ch.SourceId)
                                .CurrentChapterId);
                    });
            }));

        CreateMap<Manga, MangaUserDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.CoverUrl, opt => opt.MapFrom(src => src.CoverUrl))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

        CreateMap<UserMangaGroupByManga, MangaUserLoggedDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Manga.Id))
            .ForMember(dest => dest.CoverUrl, opt => opt.MapFrom(src => src.Manga.CoverUrl))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Manga.Name))
            .ForMember(dest => dest.Chapters, opt => opt.MapFrom((src, _, _, _) =>
            {
                return src.Manga.Chapters?
                    .Select(a => new ChapterDto(a.Id, a.SourceId, a.Source!.Name, a.Date, a.Number,
                        src.SourcesWithLastChapterRead.Any(b => b.SourceId == a.SourceId) && a.Id <=
                        src.SourcesWithLastChapterRead.First(c => c.SourceId == a.SourceId).LastChapterRead));
            }));

        CreateMap<MyAnimeListApiResponse, Manga>()
            .ForMember(dest => dest.CoverUrl, opt => opt.MapFrom(src => src.Images.JPG.LargeImageUrl))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Titles.First().Title))
            .ForMember(dest => dest.AlternativeName,
                opt => opt.MapFrom(src => src.Titles.First().Title)) //TODO: Change this implementation
            .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Authors.First().Name))
            .ForMember(dest => dest.Synopsis, opt => opt.MapFrom(src => src.Synopsis))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
            .ForMember(dest => dest.MyAnimeListId, opt => opt.MapFrom(src => src.MalId));
    }
}