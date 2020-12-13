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
    public class ActorMarksController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public ActorMarksController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: api/ActorMarks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ActorMark>>> GetActorMarks()
        {
            return await _context.ActorMarks.ToListAsync();
        }

        // GET: api/ActorMarks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ActorMark>> GetActorMark(int id)
        {
            var actorMark = await _context.ActorMarks.FindAsync(id);

            if (actorMark == null)
            {
                return NotFound();
            }

            return actorMark;
        }

        // POST: api/ActorMarks
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ActorMark>> PostActorMark(ActorMark actorMark)
        {
            var actor = await _context.Actors.FindAsync(actorMark.ActorId);
            var user = await _context.Users.FindAsync(actorMark.UserId);

            if (user == null || actor == null)
            {
                return NotFound();
            }

            actorMark.Actor = actor;
            actorMark.User = user;
            _context.ActorMarks.Add(actorMark);
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ActorMarkExists(actorMark.ActorId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetActorMark", new { id = actorMark.ActorId }, actorMark);
        }

        // DELETE: api/ActorMarks/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ActorMark>> DeleteActorMark(int id)
        {
            var actorMark = await _context.ActorMarks.FindAsync(id);
            if (actorMark == null)
            {
                return NotFound();
            }

            _context.ActorMarks.Remove(actorMark);
            await _context.SaveChangesAsync();

            return actorMark;
        }

        private bool ActorMarkExists(int id)
        {
            return _context.ActorMarks.Any(e => e.ActorId == id);
        }
    }
}
