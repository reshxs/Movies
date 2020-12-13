using Microsoft.EntityFrameworkCore;
using Movies.Models;

namespace Movies.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // One-to-many movie <- actorAssignment
            modelBuilder.Entity<Movie>()
                .HasMany(movie => movie.ActorAssignments)
                .WithOne(actorAssignment => actorAssignment.Movie)
                .IsRequired();
            
            modelBuilder.Entity<Movie>()
                .Property(m => m.Id)
                .ValueGeneratedOnAdd();

            // One-to-many actor <- actorAssignment
            modelBuilder.Entity<Actor>()
                .HasMany(actor => actor.ActorAssignments)
                .WithOne(actorAssignment => actorAssignment.Actor)
                .IsRequired();
            
            modelBuilder.Entity<Actor>()
                .Property(a => a.Id)
                .ValueGeneratedOnAdd();

            // Primary key for Actor Assignment
            modelBuilder.Entity<ActorAssignment>()
                .HasKey(a => new {a.ActorId, a.MovieId});

            // Primary key for MovieMark
            modelBuilder.Entity<MovieMark>()
                .HasKey(m => new {m.MovieId, m.UserId});

            // One-to-many movie <- movieMark
            modelBuilder.Entity<Movie>()
                .HasMany(m => m.Marks)
                .WithOne(m => m.Movie)
                .IsRequired();

            // One-to-Many user <- movieMark
            modelBuilder.Entity<User>()
                .HasMany(u => u.MovieMarks)
                .WithOne(m => m.User)
                .IsRequired();

            // Primary key for actorMark
            modelBuilder.Entity<ActorMark>()
                .HasKey(a => new {a.ActorId, a.UserId});

            // One-to-Many actor <- actorMark
            modelBuilder.Entity<Actor>()
                .HasMany(a => a.ActorMarks)
                .WithOne(a => a.Actor)
                .IsRequired();
            
            // One-to-many user <- actormarks
            modelBuilder.Entity<User>()
                .HasMany(u => u.ActorMarks)
                .WithOne(a => a.User)
                .IsRequired();
        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<ActorAssignment> ActorAssignments { get; set; }
        public DbSet<Movies.Models.User> Users { get; set; }
        public DbSet<MovieMark> MovieMarks { get; set; }
        public DbSet<ActorMark> ActorMarks { get; set; }
    }
}