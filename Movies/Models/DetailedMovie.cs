using System.Collections;
using System.Collections.Generic;

namespace Movies.Models
{
    public class DetailedMovie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string PublishDate { get; set; }
        public double Rating { get; set; }
        public IEnumerable<ListActor> Actors { get; set; }
    }
}