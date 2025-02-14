using Microsoft.EntityFrameworkCore;

namespace ITB2203Application.Model;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions options) : base(options)
    { }
    
    public DbSet<Speaker> Speakers { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<Attendee> Attendees { get; set; }
    public DbSet<Test>? Tests { get; set; }
}
