using MyCensus.Models.Events;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyCensus.DataServices.Interfaces
{
    public interface IEventsService
    {
        Task<IEnumerable<Event>> GetEvents();

        Task<Event> GetEventById(int eventId);
    }
}
