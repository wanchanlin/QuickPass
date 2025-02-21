using Microsoft.EntityFrameworkCore;
using QuickPass.Data;
using QuickPass.Interfaces;
using QuickPass.Models;
using QuickPass.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreEntityFramework.Services
{
    // The EventService class provides methods for managing event-related operations.
    // It implements IEventService interface for defining the structure of event services.
    public class EventService : IEventService
    {
        // The ApplicationDbContext provides the context for interacting with the database.
        private readonly ApplicationDbContext _context;

        // Constructor injects the ApplicationDbContext so it can be used to interact with the database.
        public EventService(ApplicationDbContext context)
        {
            _context = context;
        }

        // This method retrieves all events from the database asynchronously.
        public async Task<IEnumerable<Event>> GetEvents()
        {
            return await _context.Events.ToListAsync();
        }

        // This method retrieves a single event by its ID asynchronously.
        // It returns null if the event is not found.
        public async Task<Event?> GetEvent(int id)
        {
            return await _context.Events.FindAsync(id);
        }

        // This method creates a new event in the database asynchronously.
        // It adds the event to the database and then saves the changes.
        public async Task<ServiceResponse> CreateEvent(Event @event)
        {
            // Add the new event to the database.
            _context.Events.Add(@event);

            // Save the changes to the database.
            await _context.SaveChangesAsync();

            // Return a response indicating the event was created successfully, along with the new event ID.
            return new ServiceResponse
            {
                Status = ServiceResponse.ServiceStatus.Created,
                CreatedId = @event.EventId // Assign the newly created ID
            };
        }

        // This method updates an existing event in the database asynchronously.
        // It modifies the event entity in the context and then saves the changes.
        public async Task<ServiceResponse> UpdateEvent(Event @event)
        {
            // Mark the event entity as modified so that changes can be tracked.
            _context.Entry(@event).State = EntityState.Modified;

            // Save the changes to the database.
            await _context.SaveChangesAsync();

            // Return a response indicating the event was updated successfully.
            return new ServiceResponse { Status = ServiceResponse.ServiceStatus.Updated };
        }

        // This method deletes an event by its ID asynchronously.
        // It checks if the event exists, removes it from the database, and saves the changes.
        public async Task<ServiceResponse> DeleteEvent(int id)
        {
            // Retrieve the event from the database by its ID.
            var @event = await _context.Events.FindAsync(id);

            // If the event does not exist, return an error response.
            if (@event == null)
            {
                return new ServiceResponse
                {
                    Status = ServiceResponse.ServiceStatus.Error,
                    Messages = new List<string> { "Event not found." }
                };
            }

            // Remove the event from the database.
            _context.Events.Remove(@event);

            // Save the changes to the database.
            await _context.SaveChangesAsync();

            // Return a response indicating the event was deleted successfully.
            return new ServiceResponse { Status = ServiceResponse.ServiceStatus.Deleted };
        }
    }
}
