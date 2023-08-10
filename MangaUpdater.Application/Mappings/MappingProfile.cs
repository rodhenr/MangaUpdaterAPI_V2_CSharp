using AutoMapper;
using MangaUpdater.Application.DTOs;
using MangaUpdater.Application.Models;
using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Manga, MangaDTO>()
            .ForMember(dest => dest.CoverURL, opt => opt.MapFrom(src => src.CoverURL))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.AlternativeName, opt => opt.MapFrom(src => src.AlternativeName))
            .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author))
            .ForMember(dest => dest.Synopsis, opt => opt.MapFrom(src => src.Synopsis))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
            .ForMember(dest => dest.MyAnimeListId, opt => opt.MapFrom(src => src.MyAnimeListId))
            .ForMember(dest => dest.IsUserFollowing, opt => opt.MapFrom(src => src.UserMangas.Any()))
            .ForMember(dest => dest.Sources, opt => opt.MapFrom(src => src.MangaSources.Select(a => new SourceDTO(a.Source.Id, a.Source.Name, a.Source.BaseURL))))
            .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.MangaGenres.Select(a => a.Genre.Name)))
            .ForMember(dest => dest.Chapters, opt => opt.MapFrom((src, _, _, context) =>
            {
                var UserSourceChapterList = src.UserMangas.Select(a => new { a.SourceId, a.CurrentChapterId });

                if (src.Chapters is null)
                {
                    return null;
                }

                return src.Chapters
                        .Select(a => new ChapterDTO(a.Id, a.Source.Id, a.Source.Name, a.Date, a.Number, UserSourceChapterList.Any(b => b.SourceId == a.SourceId) && a.Id <= UserSourceChapterList.First(c => c.SourceId == a.SourceId).CurrentChapterId));
            }));

        CreateMap<Manga, MangaUserDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.CoverURL, opt => opt.MapFrom(src => src.CoverURL))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

        CreateMap<Manga, MangaUserLoggedDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.CoverURL, opt => opt.MapFrom(src => src.CoverURL))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Chapters, opt => opt.MapFrom((src, _, _, context) =>
            {
                var UserSourceChapterList = src.UserMangas.Select(a => new { a.SourceId, a.CurrentChapterId });

                if (src.Chapters is null)
                {
                    return null;
                }

                return src.Chapters
                        .Select(a => new ChapterDTO(a.Id, a.SourceId, a.Source.Name, a.Date, a.Number, UserSourceChapterList.Any(b => b.SourceId == a.SourceId) && a.Id <= UserSourceChapterList.First(c => c.SourceId == a.SourceId).CurrentChapterId));
            }));

        CreateMap<MyAnimeListAPIResponse, Manga>()
           .ForMember(dest => dest.CoverURL, opt => opt.MapFrom(src => src.Images.JPG.LargeImageUrl))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Titles.First().Title))
            .ForMember(dest => dest.AlternativeName, opt => opt.MapFrom(src => src.Titles.First().Title)) //TODO: Change this implementation
            .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Authors.First().Name))
            .ForMember(dest => dest.Synopsis, opt => opt.MapFrom(src => src.Synopsis))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
            .ForMember(dest => dest.MyAnimeListId, opt => opt.MapFrom(src => src.MalId));
    }
}
