namespace EventManagementSystem.Models
{
    public class Attendee
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public Event Event { get; set; } = null!;  // Ensure non-nullable reference
    }
}
