// Services/AttendeeService.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using EventManagementSystem.Data;
using EventManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace EventManagementSystem.Services
{
    public class AttendeeService : IAttendeeService
    {
        private readonly ApplicationDbContext _context;

        public AttendeeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Attendee>> GetAllAttendeesAsync()
        {
            return await _context.Attendees.Include(a => a.Event).ToListAsync() ?? new List<Attendee>();
        }

        public async Task<Attendee> GetAttendeeByIdAsync(int id)
        {
            return await _context.Attendees.Include(a => a.Event)
                                           .FirstOrDefaultAsync(a => a.Id == id) ?? new Attendee();
        }

        public async Task<Attendee> CreateAttendeeAsync(Attendee attendee)
        {
            _context.Attendees.Add(attendee ?? new Attendee());
            await _context.SaveChangesAsync();
            return attendee;
        }

        public async Task UpdateAttendeeAsync(Attendee attendee)
        {
            if (attendee != null)
            {
                _context.Attendees.Update(attendee);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAttendeeAsync(int id)
        {
            var attendee = await _context.Attendees.FindAsync(id);
            if (attendee != null)
                _context.Attendees.Remove(attendee);
            await _context.SaveChangesAsync();
        }
    }
}
