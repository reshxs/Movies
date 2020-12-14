using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movies.Data;
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

        // PUT: api/MovieMarks/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovieMark(int id, MovieMark movieMark)
        {
            if (id != movieMark.MovieId)
            {
                return BadRequest();
            }

            _context.Entry(movieMark).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieMarkExists(id))
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

        // POST: api/MovieMarks
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<MovieMark>> PostMovieMark(MovieMark movieMark)
        {
            _context.MovieMarks.Add(movieMark);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (MovieMarkExists(movieMark.MovieId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetMovieMark", new { id = movieMark.MovieId }, movieMark);
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

        private bool MovieMarkExists(int id)
        {
            return _context.MovieMarks.Any(e => e.MovieId == id);
        }
    }
}
