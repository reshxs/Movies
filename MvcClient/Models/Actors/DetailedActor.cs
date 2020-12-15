using System.Collections.Generic;
using MvcClient.Models.Movies;

namespace MvcClient.Models.Actors
{
    public class DetailedActor: ListActor
    {
        public IEnumerable<ListMovie> Movies { get; set; }
    }
}