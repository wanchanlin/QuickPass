using QuickPass.Models;
using QuickPass.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuickPass.Interfaces
{
    public interface IEventService
    {
        Task<IEnumerable<Event>> GetEvents();
        Task<Event?> GetEvent(int id);
        Task<ServiceResponse> CreateEvent(Event @event);
        Task<ServiceResponse> UpdateEvent(Event @event);
        Task<ServiceResponse> DeleteEvent(int id);
    }
}