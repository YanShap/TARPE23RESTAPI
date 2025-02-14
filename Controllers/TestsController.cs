using ITB2203Application.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITB2203Application.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestsController : ControllerBase
{
    private readonly DataContext _context;

    public TestsController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Test>> GetTests(string? name = null)
    {
        var query = _context.Tests!.AsQueryable();

        if (name != null)
            query = query.Where(x => x.Name != null && x.Name.ToUpper().Contains(name.ToUpper()));

        return query.ToList();
    }

    [HttpGet("{id}")]
    public ActionResult<TextReader> GetTest(int id)
    {
        var test = _context.Tests!.Find(id);

        if (test == null)
        {
            return NotFound();
        }

        return Ok(test);
    }

    [HttpPut("{id}")]
    public IActionResult PutTest(int id, Test test)
    {
        var dbTest = _context.Tests!.AsNoTracking().FirstOrDefault(x => x.Id == test.Id);
        if (id != test.Id || dbTest == null)
        {
            return NotFound();
        }

        _context.Update(test);
        _context.SaveChanges();

        return NoContent();
    }

    [HttpPost]
    public ActionResult<Test> PostTest(Test test)
    {
        var dbExercise = _context.Tests!.Find(test.Id);
        if (dbExercise == null)
        {
            _context.Add(test);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetTest), new { Id = test.Id }, test);
        }
        else
        {
            return Conflict();
        }
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteTest(int id)
    {
        var test = _context.Tests!.Find(id);
        if (test == null)
        {
            return NotFound();
        }

        _context.Remove(test);
        _context.SaveChanges();

        return NoContent();
    }
}
