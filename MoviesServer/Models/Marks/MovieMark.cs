using System;

namespace Movies.Models.Marks
{
    public class MovieMark : BaseMark
    {
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
    }
}