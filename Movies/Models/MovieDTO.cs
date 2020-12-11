using System;
using System.Collections.Generic;

namespace Movies.Models
{
    public class MovieDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime PublishDate { get; set; }
        public IEnumerable<Actor> Actors { get; set; }
    }
}