using System;
using System.Collections.Generic;
using System.Linq;
using Movies.Models.Additional;

namespace Movies.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime PublishDate { get; set; }
        public HashSet<ActorAssignment> ActorAssignments { get; set; }
        public HashSet<MovieMark> Marks { get; set; }

        public double Rating => Marks?.Average(m => m.Mark) ?? 0;

        public ListMovie ToListMovie() =>
            new ListMovie()
            {
                Id = Id,
                Title = Title,
                PublishDate = PublishDate.ToShortDateString(),
                Rating = Rating,
            };

        public DetailedMovie ToDetailedMovie()
        {
            var actors = ActorAssignments != null 
                ? ActorAssignments.Select(x => x.Actor)
                : new HashSet<Actor>();
            
            return new DetailedMovie()
            {
                Id = Id,
                Title = Title,
                PublishDate = PublishDate.ToShortDateString(),
                Rating = Rating,
                Actors = actors.Select(a => a.ToListActor())
            };
        }
    }
}