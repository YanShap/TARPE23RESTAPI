namespace ITB2203Application.Model
{
    public class Attendee
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime RegistrationTime { get; set; }
    }
}
