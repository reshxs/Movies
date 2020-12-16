using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Movies.Models;
using Movies.Models.Authentication;
using Movies.Models.Marks;

namespace Movies.Data
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<ActorAssignment> ActorAssignments { get; set; }
        public DbSet<ActorMark> ActorMarks { get; set; }
        public DbSet<MovieMark> MovieMarks { get; set; }
            
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
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
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.MovieMarks)
                .WithOne(m => m.User)
                .IsRequired();

            // Primary key for actorMark
            modelBuilder.Entity<ActorMark>()
                .HasKey(a => new {a.ActorId, a.UserId});
            
            // One-to-many actor <- movieMark
            modelBuilder.Entity<Actor>()
                .HasMany(a => a.ActorMarks)
                .WithOne(m => m.Actor)
                .IsRequired();

            // One-to-Many user <- actorMarks
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.ActorMarks)
                .WithOne(m => m.User)
                .IsRequired();

        }
    }
}