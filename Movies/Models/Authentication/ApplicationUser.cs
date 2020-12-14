using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Movies.Models.Marks;

namespace Movies.Models.Authentication
{
    public class ApplicationUser : IdentityUser
    {
        public IEnumerable<MovieMark> MovieMarks { get; set; }
        public IEnumerable<ActorMark> ActorMarks { get; set; }
    }
}