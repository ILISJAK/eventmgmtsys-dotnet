using System.Collections.Generic;
using System.Threading.Tasks;
using EventManagementSystem.Models;

namespace EventManagementSystem.Services
{
    public interface IAttendeeService
    {
        Task<IEnumerable<Attendee>> GetAllAttendeesAsync();
        Task<Attendee> GetAttendeeByIdAsync(int id);
        Task<Attendee> CreateAttendeeAsync(Attendee attendee);
        Task UpdateAttendeeAsync(Attendee attendee);
        Task DeleteAttendeeAsync(int id);
    }
}
