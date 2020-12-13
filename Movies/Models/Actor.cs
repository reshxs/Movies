using System.Collections.Generic;
using System.Linq;

namespace Movies.Models
{
    public class Actor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public HashSet<ActorAssignment> ActorAssignments { get; set; }
        public HashSet<ActorMark> ActorMarks { get; set; }

        public ListActor ToListActor() => new ListActor {Id = Id, Name = Name, Surname = Surname};

        public DetailedActor ToDetailedActor()
        {
            var movies = ActorAssignments != null
                ? ActorAssignments.Select(a => a.Movie)
                : new HashSet<Movie>();
            
            return new DetailedActor()
            {
                Id = Id,
                Name = Name,
                Surname = Surname,
                Movies = movies.Select(m => m.ToListMovie()).ToHashSet()
            };
        }
    }
}