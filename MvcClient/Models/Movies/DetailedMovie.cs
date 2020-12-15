using System.Collections.Generic;
using MvcClient.Models.Actors;

namespace MvcClient.Models.Movies
{
    public class DetailedMovie: ListMovie
    {
        public IEnumerable<ListActor> Actors { get; set; }
    }
}