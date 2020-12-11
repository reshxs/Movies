using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movies.Data;
using Movies.Models;

namespace Movies.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActorAssignmentsController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public ActorAssignmentsController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: api/ActorAssignments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ActorAssignment>>> GetActorAssignments()
        {
            return await _context.ActorAssignments.ToListAsync();
        }

        // GET: api/ActorAssignments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ActorAssignment>> GetActorAssignment(int id)
        {
            var actorAssignment = await _context.ActorAssignments.FindAsync(id);

            if (actorAssignment == null)
            {
                return NotFound();
            }

            return actorAssignment;
        }

        // PUT: api/ActorAssignments/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutActorAssignment(int id, ActorAssignment actorAssignment)
        {
            if (id != actorAssignment.ActorId)
            {
                return BadRequest();
            }

            _context.Entry(actorAssignment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActorAssignmentExists(id))
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

        // POST: api/ActorAssignments
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ActorAssignment>> PostActorAssignment(ActorAssignment actorAssignment)
        {
            actorAssignment.Actor = await _context.Actors.FindAsync(actorAssignment.ActorId);
            actorAssignment.Movie = await _context.Movies.FindAsync(actorAssignment.MovieId);

            if (actorAssignment.Actor == null || actorAssignment.Movie == null)
            {
                return NotFound();
            }
            
            await _context.ActorAssignments.AddAsync(actorAssignment);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ActorAssignmentExists(actorAssignment.ActorId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetActorAssignment", new { id = actorAssignment.ActorId }, actorAssignment);
        }

        // DELETE: api/ActorAssignments/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ActorAssignment>> DeleteActorAssignment(int id)
        {
            var actorAssignment = await _context.ActorAssignments.FindAsync(id);
            if (actorAssignment == null)
            {
                return NotFound();
            }

            _context.ActorAssignments.Remove(actorAssignment);
            await _context.SaveChangesAsync();

            return actorAssignment;
        }

        private bool ActorAssignmentExists(int id)
        {
            return _context.ActorAssignments.Any(e => e.ActorId == id);
        }
    }
}
