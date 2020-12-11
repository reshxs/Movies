using System.Collections.Generic;
using System.Linq;

namespace Movies.Models
{
    public class Actor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public IEnumerable<Like<Actor>> Likes { get; set; }
        public float Rating => Likes?.Sum(l => l.Mark) ?? 0;
        public IEnumerable<ActorAssignment> ActorAssignments { get; set; }
    }
}