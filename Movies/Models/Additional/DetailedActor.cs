using System.Collections.Generic;

namespace Movies.Models.Additional
{
    public class DetailedActor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public HashSet<ListMovie> Movies { get; set; }
        public HashSet<ActorMark> ActorMarks { get; set; }
    }
}