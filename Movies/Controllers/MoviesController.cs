using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movies.Data;
using Movies.Models;
using Movies.Models.Additional;
using Movies.Models.Authentication;

namespace Movies.Controllers
{
    [Route("api/Movies")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public MoviesController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: api/Movies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ListMovie>>> GetMovies()
        {
            return await _context.Movies
                .Select(m => m.ToListMovie())
                .ToListAsync();
        }

        [HttpGet("OrderByDate")]
        public async Task<ActionResult<IEnumerable<ListMovie>>> GetMoviesByDate()
        {
            return await _context.Movies
                .OrderBy(m => m.PublishDate)
                .Select(m => m.ToListMovie())
                .ToListAsync();
        }

        [HttpGet("OrderByTitle")]
        public async Task<ActionResult<IEnumerable<ListMovie>>> GetMoviesByTitle()
        {
            return await _context.Movies
                .OrderBy(m => m.Title)
                .Select(m => m.ToListMovie())
                .ToListAsync();
        }
        
        [HttpGet("OrderByRating")]
        public async Task<ActionResult<IEnumerable<ListMovie>>> GetMoviesByRating()
        {
            return await _context.Movies
                .Select(m => m.ToListMovie())
                .OrderBy(m => m.Rating)
                .ToListAsync();
        }

        // GET: api/Movies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DetailedMovie>> GetMovie(int id)
        {
            var movie = await _context.Movies
                    .Include(m =>m.ActorAssignments)
                        .ThenInclude(a => a.Actor)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
            {
                return NotFound();
            }

            return movie.ToDetailedMovie();
        }

        // PUT: api/Movies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = UserRoles.Admin)]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovie(int id, Movie movie)
        {
            if (id != movie.Id)
            {
                return BadRequest();
            }

            _context.Entry(movie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Movies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost]
        public async Task<ActionResult<Movie>> PostMovie(Movie movie)
        {
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMovie), new { id = movie.Id }, movie);
        }

        // DELETE: api/Movies/5
        [Authorize(Roles = UserRoles.Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }
    }
}
