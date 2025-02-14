using ITB2203Application.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITB2203Application.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttendeesController : ControllerBase
    {
        private readonly DataContext _context;

        public AttendeesController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Attendee>> GetAttendees(int? daysBeforeEvent = null)
        {
            var query = _context.Attendees.AsQueryable();

            if (daysBeforeEvent.HasValue)
            {
                query = query.Where(a =>
                    _context.Events
                        .Where(e => e.Id == a.EventId)
                        .Any(e => a.RegistrationTime < e.Date.AddDays(-daysBeforeEvent.Value))
                );
            }

            return Ok(query.ToList());
        }


        [HttpGet("{id}")]
        public ActionResult<Attendee> GetAttendee(int id)
        {
            var attendee = _context.Attendees!.Find(id);

            if (attendee == null)
            {
                return NotFound();
            }

            return Ok(attendee);
        }

        [HttpPut("{id}")]
        public IActionResult PutAttendee(int id, Attendee attendee)
        {
            if (id != attendee.Id)
            {
                return BadRequest("ID mismatch.");
            }

            var existingAttendee = _context.Attendees!.AsNoTracking().FirstOrDefault(a => a.Id == id);
            if (existingAttendee == null)
            {
                return NotFound();
            }

            _context.Update(attendee);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPost]
        public ActionResult<Attendee> PostAttendee(Attendee attendee)
        {
            var eventDetails = _context.Events!.FirstOrDefault(e => e.Id == attendee.EventId);
            if (eventDetails == null)
            {
                return NotFound("Event not found.");
            }

            var speaker = _context.Speakers!.FirstOrDefault(s => s.Id == eventDetails.SpeakerId);

            if (!attendee.Email.Contains('@'))
            {
                return BadRequest("Invalid email format.");
            }

            if (attendee.RegistrationTime >= eventDetails.Date)
            {
                return BadRequest("Registration time cannot be after event date.");
            }

            if (_context.Attendees!.Any(a => a.Email == attendee.Email))
            {
                return BadRequest("Attendee with this email already exists.");
            }

            if (speaker != null && speaker.Email == attendee.Email)
            {
                return BadRequest("Speaker cannot register as an attendee.");
            }

            attendee.Id = 0;
            _context.Attendees.Add(attendee);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetAttendee), new { id = attendee.Id }, attendee);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAttendee(int id)
        {
            var attendee = _context.Attendees!.Find(id);
            if (attendee == null)
            {
                return NotFound();
            }

            _context.Remove(attendee);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
