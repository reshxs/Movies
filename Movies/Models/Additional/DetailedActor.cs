using System.Collections.Generic;
using Movies.Models.Marks;

namespace Movies.Models.Additional
{
    public class DetailedActor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public double Rating { get; set; }
        public HashSet<ListMovie> Movies { get; set; }
        public HashSet<ActorMark> ActorMarks { get; set; }
    }
}