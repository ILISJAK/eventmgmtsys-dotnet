// Models/Event.cs
using System;
using System.Collections.Generic;

namespace EventManagementSystem.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string? EventName { get; set; }
        public string? Location { get; set; }
        public string? Organizer { get; set; }
        public DateTime Date { get; set; }
        public ICollection<Attendee> Attendees { get; set; } = new List<Attendee>();
    }
}