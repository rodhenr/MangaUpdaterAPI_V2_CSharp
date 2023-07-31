using AutoMapper;
using MangaUpdater.Application.DTOs;
using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Application.Mappings;

public class MangaToDTOMappingProfile : Profile
{
    public MangaToDTOMappingProfile()
    {
        CreateMap<Manga, MangaDTO>()
            .ForMember(dest => dest.CoverURL, opt => opt.MapFrom(src => src.CoverURL))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.AlternativeName, opt => opt.MapFrom(src => src.AlternativeName))
            .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author))
            .ForMember(dest => dest.Synopsis, opt => opt.MapFrom(src => src.Synopsis))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
            .ForMember(dest => dest.MyAnimeListURL, opt => opt.MapFrom(src => src.MyAnimeListURL))
            .ForMember(dest => dest.IsUserFollowing, opt => opt.MapFrom(src => src.UserMangas.Any()))
            .ForMember(dest => dest.Sources, opt => opt.MapFrom(src => src.MangaSources.Select(a => new SourceDTO(a.Source.Name, a.Source.BaseURL))))
            .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.MangaGenres.Select(a => a.Genre.Name)))
            .ForMember(dest => dest.Chapters, opt => opt.MapFrom((src, _, _, context) =>
            {
                var UserSourceChapterList = src.UserMangas.Select(a => new { a.SourceId, a.CurrentChapterId });

                if(src.Chapters is null)
                {
                    return null;
                }

                return src.Chapters
                        .Select(a => new ChapterDTO(a.Id, a.Source.Id, a.Source.Name, a.Date, a.Number, UserSourceChapterList.Any(b => b.SourceId == a.SourceId) && a.Id <= UserSourceChapterList.First(c => c.SourceId == a.SourceId).CurrentChapterId));
            }));
    }
}
