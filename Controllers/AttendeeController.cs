using Microsoft.AspNetCore.Mvc;
using EventManagementSystem.Services;
using EventManagementSystem.Models;
using System.Threading.Tasks;

namespace EventManagementSystem.Controllers
{
    public class AttendeeController : Controller
    {
        private readonly IAttendeeService _attendeeService;

        public AttendeeController(IAttendeeService attendeeService)
        {
            _attendeeService = attendeeService;
        }

        public async Task<IActionResult> Index()
        {
            var attendees = await _attendeeService.GetAllAttendeesAsync();
            return View(attendees);
        }

        public async Task<IActionResult> Details(int id)
        {
            var attendee = await _attendeeService.GetAttendeeByIdAsync(id);
            if (attendee == null)
            {
                return NotFound();
            }
            return View(attendee);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Attendee attendee)
        {
            if (ModelState.IsValid)
            {
                await _attendeeService.CreateAttendeeAsync(attendee);
                return RedirectToAction(nameof(Index));
            }
            return View(attendee);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var attendee = await _attendeeService.GetAttendeeByIdAsync(id);
            if (attendee == null)
            {
                return NotFound();
            }
            return View(attendee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Attendee attendee)
        {
            if (id != attendee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _attendeeService.UpdateAttendeeAsync(attendee);
                return RedirectToAction(nameof(Index));
            }
            return View(attendee);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var attendee = await _attendeeService.GetAttendeeByIdAsync(id);
            if (attendee == null)
            {
                return NotFound();
            }
            return View(attendee);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _attendeeService.DeleteAttendeeAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
