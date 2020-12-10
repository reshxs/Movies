using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Movies.Models
{
    public class Movie
    {
        public int Id { get; set; }
        
        public string Title { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime PublishDate { get; set; }
        
        public IEnumerable<ActorAssignment> ActorAssignments { get; set; }
    }
}