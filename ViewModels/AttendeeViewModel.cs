// ViewModels/AttendeeViewModel.cs
using System.Collections.Generic;
using EventManagementSystem.Models;

namespace EventManagementSystem.ViewModels
{
    public class AttendeeViewModel
    {
        public Attendee? Attendee { get; set; }
        public List<Event>? Events { get; set; } = new List<Event>();
    }
}
