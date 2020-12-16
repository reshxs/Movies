using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movies.Data;
using Movies.Models;
using Movies.Models.Authentication;
using Movies.Models.Marks;

namespace Movies.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MovieMarksController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public MovieMarksController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: api/MovieMarks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieMark>> GetMovieMark(int id)
        {
            var user = await GetUserAsync();
            if (user == null)
            {
                return Unauthorized();
            }
            
            var movieMark = await _context.MovieMarks
                .FirstOrDefaultAsync(m => m.MovieId == id && m.UserId == user.Id);

            if (movieMark == null)
            {
                return NotFound();
            }

            return movieMark;
        }

        // POST: api/MovieMarks
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<MovieMark>> PostMovieMark(MovieMark movieMark)
        {
            var user = await GetUserAsync();
            var movie = await _context.Movies.FindAsync(movieMark.MovieId);
            if (movie == null)
            {
                return NotFound();
            }

            if (user == null)
            {
                return Unauthorized();
            }
            
            AddMovieRating(movie, movieMark.Mark);
            movieMark.User = user;
            movieMark.Movie = movie;

            await _context.MovieMarks.AddAsync(movieMark);
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (MovieMarkExists(movieMark.MovieId, movieMark.UserId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction(nameof(GetMovieMark), new { id = movieMark.MovieId }, movieMark);
        }

        private void AddMovieRating(Movie movie, int mark)
        {
            if (movie.Marks == null)
            {
                movie.Rating = mark;
            }
            else
            {
                var marksCount = movie.Marks.Count;
                movie.Rating = (movie.Rating / marksCount + mark) / (marksCount + 1);
            }
        }

        private async Task<ApplicationUser> GetUserAsync()
        {
            var userName = User.Identity?.Name;
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == userName);
            return user;
        }
        
        private bool MovieMarkExists(int movieId, string userId)
        {
            return _context.MovieMarks.Any(e => e.MovieId == movieId && e.UserId == userId);
        }
    }
}
