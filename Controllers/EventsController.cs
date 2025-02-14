using ITB2203Application.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITB2203Application.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly DataContext _context;

        public EventsController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Event>> GetEvents(string? name = null, string? location = null)
        {
            var query = _context.Events.AsQueryable();

            if (name != null)
                query = query.Where(x => x.Name != null && x.Name.ToUpper().Contains(name.ToUpper()));

            if (location != null)
                query = query.Where(x => x.Location != null && x.Location.ToUpper().Contains(location.ToUpper()));

            return query.ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<Event> GetEvent(int id)
        {
            var @event = _context.Events!.Find(id);

            if (@event == null)
            {
                return NotFound();
            }

            return Ok(@event);
        }

        [HttpPut("{id}")]
        public IActionResult PutEvent(int id, Event @event)
        {
            var dbEvent = _context.Events.AsNoTracking().FirstOrDefault(x => x.Id == @event.Id);
            if (id != @event.Id || dbEvent == null)
            {
                return NotFound();
            }

            _context.Update(@event);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPost]
        public ActionResult<Event> PostEvent(Event @event)
        {
            var speakerExists = _context.Speakers!.Any(s => s.Id == @event.SpeakerId);
            if (!speakerExists)
            {
                return NotFound("Speaker not found.");
            }

            _context.Events!.Add(@event);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetEvent), new { Id = @event.Id }, @event);
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteEvent(int id)
        {
            var @event = _context.Events.Find(id);
            if (@event == null)
            {
                return NotFound();
            }

            _context.Remove(@event);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
