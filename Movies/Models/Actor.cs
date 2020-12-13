using System.Collections.Generic;

namespace Movies.Models
{
    public class Actor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public HashSet<ActorAssignment> ActorAssignments { get; set; }
        public HashSet<ActorMark> ActorMarks { get; set; }
    }
}