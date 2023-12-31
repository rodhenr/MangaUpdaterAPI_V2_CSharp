﻿using System.Text.Json.Serialization;

namespace MangaUpdater.Domain.Entities;

public sealed class Manga : Entity
{
    public required string CoverUrl { get; set; }
    public required string Synopsis { get; set; }
    public required string Type { get; set; }
    public required int MyAnimeListId { get; set; }

    [JsonIgnore] public IEnumerable<Chapter>? Chapters { get; set; }
    [JsonIgnore] public IEnumerable<UserManga>? UserMangas { get; set; }
    [JsonIgnore] public IEnumerable<MangaAuthor>? MangaAuthors { get; set; }
    [JsonIgnore] public IEnumerable<MangaGenre>? MangaGenres { get; set; }
    [JsonIgnore] public IEnumerable<MangaSource>? MangaSources { get; set; }
    [JsonIgnore] public IEnumerable<MangaTitle>? MangaTitles { get; set; }
}