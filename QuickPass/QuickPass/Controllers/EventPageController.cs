using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using QuickPass.Interfaces;
using QuickPass.Models.ViewModels;
using System.Diagnostics;
using QuickPass.Models;

namespace QuickPass.Controllers
{
    public class EventPageController : Controller
    {
        private readonly IEventService _eventService;

        public EventPageController(IEventService eventService)
        {
            _eventService = eventService;
        }



        // GET: EventsPage
        public async Task<IActionResult> List()
        {
            var events = await _eventService.GetEvents();
            return View(events);
        }

        // GET: EventsPage/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var @event = await _eventService.GetEvent(id);
            if (@event == null)
            {
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    Errors = new List<string> { "Event not found." }
                });
            }
            return View(@event);
        }

        //GET: EventsPage/Create
       [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: EventsPage/Create
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,Venue,Date,TotalTickets")] Event @event)
        {
            if (ModelState.IsValid)
            {
                var response = await _eventService.CreateEvent(@event);
                if (response.Status == ServiceResponse.ServiceStatus.Created)
                {
                    return RedirectToAction("Details", new { id = response.CreatedId }); // Use CreatedId
                }
                return View("Error", new ErrorViewModel { Errors = response.Messages });
            }
            return View(@event);
        }

        // GET: EventsPage/Edit/
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var @event = await _eventService.GetEvent(id);
            if (@event == null)
            {
                return NotFound();
            }
            return View(@event);
        }

        // POST: EventsPage/Edit/5
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EventId,Name,Description,Venue,Date,TotalTickets")] Event @event)
        {
            if (id != @event.EventId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var response = await _eventService.UpdateEvent(@event);
                if (response.Status == ServiceResponse.ServiceStatus.Updated)
                {
                    return RedirectToAction(nameof(Index));
                }
                return View("Error", new ErrorViewModel { Errors = response.Messages });
            }
            return View(@event);
        }

        // GET: EventsPage/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var @event = await _eventService.GetEvent(id);
            if (@event == null)
            {
                return NotFound();
            }
            return View(@event);
        }

        // POST: EventsPage/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _eventService.DeleteEvent(id);
            if (response.Status == ServiceResponse.ServiceStatus.Deleted)
            {
                return RedirectToAction(nameof(Index));
            }
            return View("Error", new ErrorViewModel { Errors = response.Messages });
        }
    }
}