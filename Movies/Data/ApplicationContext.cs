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

            // One-to-many actor <- actorAssignment
            modelBuilder.Entity<Actor>()
                .HasMany(actor => actor.ActorAssignments)
                .WithOne(actorAssignment => actorAssignment.Actor)
                .IsRequired();

            // Primary key for Actor Assignment
            modelBuilder.Entity<ActorAssignment>()
                .HasKey(a => new {a.ActorId, a.MovieId});

            modelBuilder.Entity<Like<Movie>>()
                .HasKey(m => new {m.UserId, m.ObjectId});

            modelBuilder.Entity<Like<Actor>>()
                .HasKey(m => new {m.UserId, m.ObjectId});
        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<ActorAssignment> ActorAssignments { get; set; }
        public DbSet<Movies.Models.User> Users { get; set; }
        public DbSet<Like<Movie>> MovieLikes { get; set; }
        public DbSet<Like<Actor>> ActorLikes { get; set; }
    }
}