using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movies.Data;
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

        // GET: api/MovieMarks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieMark>>> GetMovieMarks()
        {
            return await _context.MovieMarks.ToListAsync();
        }

        // GET: api/MovieMarks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieMark>> GetMovieMark(int id)
        {
            var movieMark = await _context.MovieMarks.FindAsync(id);

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
            var currentUserName = User.Identity.Name;
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == currentUserName);
            var movie = await _context.Movies.FindAsync(movieMark.MovieId);
            if (movie == null)
            {
                return NotFound();
            }

            if (user == null)
            {
                return Unauthorized();
            }
            
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

        // DELETE: api/MovieMarks/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<MovieMark>> DeleteMovieMark(int id)
        {
            var movieMark = await _context.MovieMarks.FindAsync(id);
            if (movieMark == null)
            {
                return NotFound();
            }

            _context.MovieMarks.Remove(movieMark);
            await _context.SaveChangesAsync();

            return movieMark;
        }

        private bool MovieMarkExists(int movieId, string userId)
        {
            return _context.MovieMarks.Any(e => e.MovieId == movieId && e.UserId == userId);
        }
    }
}
