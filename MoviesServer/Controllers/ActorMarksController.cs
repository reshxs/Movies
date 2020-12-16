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
    public class ActorMarksController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public ActorMarksController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: api/ActorMarks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ActorMark>> GetActorMark(int id)
        {
            var user = await GetUserAsync();

            if (user == null)
            {
                return Unauthorized();
            }
            
            var actorMark = await _context.ActorMarks
                .FirstOrDefaultAsync(m => m.ActorId == id && m.UserId == user.Id);

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
            var user = await GetUserAsync();

            if (actor == null)
            {
                return NotFound();
            }

            if (user == null)
            {
                return Unauthorized();
            }

            AddActorRating(actor, actorMark.Mark);
            actorMark.User = user;
            actorMark.Actor = actor;
            await _context.ActorMarks.AddAsync(actorMark);
            
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

            return CreatedAtAction(nameof(GetActorMark), new { id = actorMark.ActorId }, actorMark);
        }

        private void AddActorRating(Actor actor, int mark)
        {
            if (actor.ActorMarks == null)
            {
                actor.Rating = mark;
            }
            else
            {
                var marksCount = actor.ActorMarks.Count;
                actor.Rating = (actor.Rating / marksCount + mark) / (marksCount + 1);
            }
        }

        private async Task<ApplicationUser> GetUserAsync()
        {
            var userName = User.Identity?.Name;
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == userName);
            return user;
        }

        private bool ActorMarkExists(int id)
        {
            return _context.ActorMarks.Any(e => e.ActorId == id);
        }
    }
}
