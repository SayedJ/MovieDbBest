using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using webapp_cloudrun.Models;

namespace webapp_cloudrun.Context;

public partial class MovieDbContext : DbContext
{
    public MovieDbContext()
    {
    }

    public MovieDbContext(DbContextOptions<MovieDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Director> Directors { get; set; }

    public virtual DbSet<Movie> Movies { get; set; }

    public virtual DbSet<Person> People { get; set; }

    public virtual DbSet<Rating> Ratings { get; set; }

    public virtual DbSet<Star> Stars { get; set; }
    public virtual DbSet<ImageUrl> Imageurl { get; set; }
    public virtual DbSet<MyFavMovies> FavMovies { get; set; }
    public virtual DbSet<UserLogin> FilmAddict { get; set; }
    public virtual DbSet<User> FilmUser { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
       .SetBasePath(Directory.GetCurrentDirectory())
       .AddJsonFile("appsettings.json")
       .Build();

        string connectionString = configuration.GetConnectionString("Default");

        optionsBuilder.UseSqlServer(connectionString);
    }
        
    //=> optionsBuilder.UseSqlServer("Data Source=104.154.19.42; database = moviedb; User ID=sqlserver;Password=4335;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {


        modelBuilder.Entity<Director>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("directors");

            entity.Property(e => e.MovieId).HasColumnName("movie_id");
            entity.Property(e => e.PersonId).HasColumnName("person_id");
        });

        modelBuilder.Entity<Movie>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("movies");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Title)
                .HasColumnType("text")
                .HasColumnName("title");
            entity.Property(e => e.Year).HasColumnName("year");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("people");

            entity.Property(e => e.Birth).HasColumnName("birth");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasColumnType("text")
                .HasColumnName("name");
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("ratings");

            entity.Property(e => e.MovieId).HasColumnName("movie_id");
            entity.Property(e => e.Rating1).HasColumnName("rating");
            entity.Property(e => e.Votes).HasColumnName("votes");
        });

        modelBuilder.Entity<Star>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("stars");

            entity.Property(e => e.MovieId).HasColumnName("movie_id");
            entity.Property(e => e.PersonId).HasColumnName("person_id");
        });

        modelBuilder.Entity<ImageUrl>(entity =>
        {
            entity.ToTable("imageUrl");
            entity.HasKey(x => x.ImageId);
            entity.Property(e => e.ImageId).HasColumnName("id");
            entity.Property(e => e.MovieId).HasColumnName("movie_id");
            entity.Property(e => e.Url).HasColumnName("image_url");
        });    
        modelBuilder.Entity<MyFavMovies>(entity =>
        {
            entity.ToTable("FavMovies");
            entity.HasKey(x => x.Id);
            entity.Property(e => e.userId).HasColumnName("user_id");
            entity.Property(e => e.MovieId).HasColumnName("movie_id");
          
        });    
        modelBuilder.Entity<UserLogin>(entity =>
        {
            entity.ToTable("FilmAddict");
            entity.HasKey(x => x.UserId);
            entity.Property(e => e.Username).HasColumnName("username");
            entity.Property(e => e.Password).HasColumnName("password");
            entity.Property(e => e.ConfirmPassword).HasColumnName("confirm_password");
        }); 
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("FilmUser");
            entity.HasKey(x => x.Id);
            entity.Property(e => e.Username).HasColumnName("username");
            entity.Property(e => e.Password).HasColumnName("password");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Role).HasColumnName("role");

        });
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
