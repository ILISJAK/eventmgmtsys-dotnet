using Microsoft.AspNetCore.Mvc;
using EventManagementSystem.Services;
using EventManagementSystem.Models;
using EventManagementSystem.ViewModels;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EventManagementSystem.Controllers
{
    public class AttendeeController : Controller
    {
        private readonly IAttendeeService _attendeeService;
        private readonly IEventService _eventService;
        private readonly ILogger<AttendeeController> _logger;

        public AttendeeController(IAttendeeService attendeeService, IEventService eventService, ILogger<AttendeeController> logger)
        {
            _attendeeService = attendeeService;
            _eventService = eventService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Index action called.");
            var attendees = await _attendeeService.GetAllAttendeesAsync();
            _logger.LogInformation($"Retrieved {attendees.Count()} attendees.");
            return View(attendees);
        }

        public async Task<IActionResult> Details(int id)
        {
            _logger.LogInformation($"Details action called with id: {id}");
            var attendee = await _attendeeService.GetAttendeeByIdAsync(id);
            if (attendee == null)
            {
                _logger.LogWarning($"Attendee with id: {id} not found.");
                return NotFound();
            }
            _logger.LogInformation($"Retrieved attendee: {attendee.Name}");
            return View(attendee);
        }

        public async Task<IActionResult> Create()
        {
            _logger.LogInformation("Create GET action called.");
            var events = await _eventService.GetAllEventsAsync();
            if (events == null || !events.Any())
            {
                _logger.LogWarning("No events available.");
                ModelState.AddModelError("", "No events available.");
                events = new List<Event>(); // Ensure events is not null
            }
            else
            {
                _logger.LogInformation($"Retrieved {events.Count()} events.");
            }

            var viewModel = new AttendeeViewModel
            {
                Attendee = new Attendee(),
                Events = events.ToList()
            };

            _logger.LogInformation($"ViewModel created with {viewModel.Events.Count} events.");

            ViewBag.Events = new SelectList(viewModel.Events, "Id", "EventName");

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AttendeeViewModel viewModel)
        {
            _logger.LogInformation("Create POST action called.");

            if (viewModel.Attendee == null)
            {
                _logger.LogWarning("Attendee property is null in the incoming view model.");
                ModelState.AddModelError(string.Empty, "Attendee information is required.");
            }
            else
            {
                // Log incoming model values
                _logger.LogInformation($"Incoming Attendee Name: {viewModel.Attendee.Name}");
                _logger.LogInformation($"Incoming Attendee Email: {viewModel.Attendee.Email}");
                _logger.LogInformation($"Incoming Attendee PhoneNumber: {viewModel.Attendee.PhoneNumber}");
                _logger.LogInformation($"Incoming Attendee EventId: {viewModel.Attendee.EventId}");
            }

            _logger.LogInformation($"ModelState.IsValid: {ModelState.IsValid}");

            if (ModelState.IsValid)
            {
                _logger.LogInformation("Model state is valid. Creating attendee.");
                await _attendeeService.CreateAttendeeAsync(viewModel.Attendee);
                _logger.LogInformation($"Attendee {viewModel.Attendee.Name} created successfully.");
                return RedirectToAction(nameof(Index));
            }

            _logger.LogWarning("Model state is invalid.");
            foreach (var state in ModelState)
            {
                foreach (var error in state.Value.Errors)
                {
                    _logger.LogError($"Property: {state.Key} Error: {error.ErrorMessage}");
                }
            }

            var events = await _eventService.GetAllEventsAsync() ?? new List<Event>();
            viewModel.Events = events.ToList();
            ViewBag.Events = new SelectList(events, "Id", "EventName");

            _logger.LogInformation($"ViewModel updated with {viewModel.Events.Count} events after validation failed.");

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AttendeeViewModel viewModel)
        {
            _logger.LogInformation($"Edit POST action called with id: {id}");
            if (id != viewModel.Attendee.Id)
            {
                _logger.LogWarning($"Mismatched attendee id: {id} vs {viewModel.Attendee.Id}");
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _logger.LogInformation("Model state is valid. Updating attendee.");
                await _attendeeService.UpdateAttendeeAsync(viewModel.Attendee);
                _logger.LogInformation($"Attendee {viewModel.Attendee.Name} updated successfully.");
                return RedirectToAction(nameof(Index));
            }

            _logger.LogWarning("Model state is invalid.");
            var events = await _eventService.GetAllEventsAsync() ?? new List<Event>();
            viewModel.Events = events.ToList();
            ViewBag.Events = new SelectList(events, "Id", "EventName");

            _logger.LogInformation($"ViewModel updated with {viewModel.Events.Count} events after validation failed.");

            return View(viewModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation($"Delete GET action called with id: {id}");
            var attendee = await _attendeeService.GetAttendeeByIdAsync(id);
            if (attendee == null)
            {
                _logger.LogWarning($"Attendee with id: {id} not found.");
                return NotFound();
            }
            _logger.LogInformation($"Retrieved attendee: {attendee.Name}");
            return View(attendee);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogInformation($"Delete POST action called with id: {id}");
            await _attendeeService.DeleteAttendeeAsync(id);
            _logger.LogInformation($"Attendee with id: {id} deleted successfully.");
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> GetAttendeesByEvent(int eventId)
        {
            var attendees = await _attendeeService.GetAllAttendeesAsync();
            var filteredAttendees = attendees.Where(a => a.EventId == eventId).ToList();
            return View(filteredAttendees);
        }

        public async Task<IActionResult> GetAttendeesByEmail(string email)
        {
            var attendees = await _attendeeService.GetAllAttendeesAsync();
            var filteredAttendees = attendees.Where(a => a.Email == email).ToList();
            return View(filteredAttendees);
        }
    }
}
