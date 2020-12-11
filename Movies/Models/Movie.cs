using System;
using System.Collections.Generic;
using System.Linq;

namespace Movies.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime PublishDate { get; set; }
        public IEnumerable<ActorAssignment> ActorAssignments { get; set; }
        public IEnumerable<Like<Movie>> Likes { get; set; }

        public float Rating => Likes?.Sum(l => l.Mark) ?? 0;

        public MovieDto ToMovieDto()
        {
            var actors = ActorAssignments != null 
                ? ActorAssignments.Select(x => x.Actor)
                : new HashSet<Actor>();
            
            return new MovieDto()
            {
                Id = Id,
                Title = Title,
                PublishDate = PublishDate,
                Actors = actors
            };
        }
    }
}