using System;

namespace Movies.Models.Marks
{
    public class ActorMark : BaseMark
    {
        public int ActorId { get; set; }
        public Actor Actor { get; set; }
    }
}