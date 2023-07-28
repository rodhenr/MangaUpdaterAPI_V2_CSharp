﻿// <auto-generated />
using System;
using MangaUpdater.Infra.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MangaUpdater.Infra.Data.Migrations
{
    [DbContext(typeof(MangaUpdaterContext))]
    partial class MangaUpdaterContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MangaUpdater.Domain.Entities.Chapter", b =>
                {
                    b.Property<int>("MangaId")
                        .HasColumnType("int");

                    b.Property<int>("SourceId")
                        .HasColumnType("int");

                    b.Property<float>("Number")
                        .HasColumnType("real");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.HasKey("MangaId", "SourceId", "Number");

                    b.HasIndex("SourceId");

                    b.ToTable("Chapters");
                });

            modelBuilder.Entity("MangaUpdater.Domain.Entities.Genre", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Id");

                    b.ToTable("Genres");
                });

            modelBuilder.Entity("MangaUpdater.Domain.Entities.Manga", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AlternativeName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Author")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("CoverURL")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("MyAnimeListURL")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Synopsis")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Id");

                    b.ToTable("Mangas");
                });

            modelBuilder.Entity("MangaUpdater.Domain.Entities.MangaGenre", b =>
                {
                    b.Property<int>("MangaId")
                        .HasColumnType("int");

                    b.Property<int>("GenreId")
                        .HasColumnType("int");

                    b.HasKey("MangaId", "GenreId");

                    b.HasIndex("GenreId");

                    b.ToTable("MangaGenres");
                });

            modelBuilder.Entity("MangaUpdater.Domain.Entities.MangaSource", b =>
                {
                    b.Property<int>("MangaId")
                        .HasColumnType("int");

                    b.Property<int>("SourceId")
                        .HasColumnType("int");

                    b.Property<string>("URL")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("MangaId", "SourceId");

                    b.HasIndex("SourceId");

                    b.ToTable("MangaSources");
                });

            modelBuilder.Entity("MangaUpdater.Domain.Entities.Source", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("BaseURL")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Sources");
                });

            modelBuilder.Entity("MangaUpdater.Domain.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Avatar")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("MangaUpdater.Domain.Entities.UserManga", b =>
                {
                    b.Property<int>("MangaId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("SourceId")
                        .HasColumnType("int");

                    b.Property<int>("ChapterId")
                        .HasColumnType("int");

                    b.Property<float>("LastChapter")
                        .HasColumnType("real");

                    b.HasKey("MangaId", "UserId", "SourceId", "ChapterId");

                    b.HasIndex("SourceId");

                    b.HasIndex("UserId");

                    b.HasIndex("MangaId", "SourceId", "LastChapter");

                    b.ToTable("UserMangas");
                });

            modelBuilder.Entity("MangaUpdater.Domain.Entities.Chapter", b =>
                {
                    b.HasOne("MangaUpdater.Domain.Entities.Manga", "Manga")
                        .WithMany("Chapters")
                        .HasForeignKey("MangaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MangaUpdater.Domain.Entities.Source", "Source")
                        .WithMany("Chapters")
                        .HasForeignKey("SourceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Manga");

                    b.Navigation("Source");
                });

            modelBuilder.Entity("MangaUpdater.Domain.Entities.MangaGenre", b =>
                {
                    b.HasOne("MangaUpdater.Domain.Entities.Genre", "Genre")
                        .WithMany("MangaGenres")
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MangaUpdater.Domain.Entities.Manga", "Manga")
                        .WithMany("MangaGenres")
                        .HasForeignKey("MangaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Genre");

                    b.Navigation("Manga");
                });

            modelBuilder.Entity("MangaUpdater.Domain.Entities.MangaSource", b =>
                {
                    b.HasOne("MangaUpdater.Domain.Entities.Manga", "Manga")
                        .WithMany("MangaSources")
                        .HasForeignKey("MangaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MangaUpdater.Domain.Entities.Source", "Source")
                        .WithMany("MangaSources")
                        .HasForeignKey("SourceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Manga");

                    b.Navigation("Source");
                });

            modelBuilder.Entity("MangaUpdater.Domain.Entities.UserManga", b =>
                {
                    b.HasOne("MangaUpdater.Domain.Entities.Manga", "Manga")
                        .WithMany("UserMangas")
                        .HasForeignKey("MangaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MangaUpdater.Domain.Entities.Source", "Source")
                        .WithMany("UserMangas")
                        .HasForeignKey("SourceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MangaUpdater.Domain.Entities.User", "User")
                        .WithMany("UserMangas")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MangaUpdater.Domain.Entities.Chapter", "Chapter")
                        .WithMany("UserMangas")
                        .HasForeignKey("MangaId", "SourceId", "LastChapter")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Chapter");

                    b.Navigation("Manga");

                    b.Navigation("Source");

                    b.Navigation("User");
                });

            modelBuilder.Entity("MangaUpdater.Domain.Entities.Chapter", b =>
                {
                    b.Navigation("UserMangas");
                });

            modelBuilder.Entity("MangaUpdater.Domain.Entities.Genre", b =>
                {
                    b.Navigation("MangaGenres");
                });

            modelBuilder.Entity("MangaUpdater.Domain.Entities.Manga", b =>
                {
                    b.Navigation("Chapters");

                    b.Navigation("MangaGenres");

                    b.Navigation("MangaSources");

                    b.Navigation("UserMangas");
                });

            modelBuilder.Entity("MangaUpdater.Domain.Entities.Source", b =>
                {
                    b.Navigation("Chapters");

                    b.Navigation("MangaSources");

                    b.Navigation("UserMangas");
                });

            modelBuilder.Entity("MangaUpdater.Domain.Entities.User", b =>
                {
                    b.Navigation("UserMangas");
                });
#pragma warning restore 612, 618
        }
    }
}
