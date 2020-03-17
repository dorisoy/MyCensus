using MyCensus.DataServices.Base;
using MyCensus.DataServices.Interfaces;
using MyCensus.Models.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCensus.DataServices
{
    public class EventsService : IEventsService
    {
        private readonly IRequestProvider _requestProvider;

        public EventsService(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }

        public async Task<IEnumerable<Event>> GetEvents()
        {
            //UriBuilder builder = new UriBuilder(GlobalSettings.EventsEndpoint);
            //builder.Path = "api/Events";
            //string uri = builder.ToString();
            //IEnumerable<Event> events = await _requestProvider.GetAsync<IEnumerable<Event>>(uri);
            await Task.Delay(100);
            var events = new List<Event>() {
                new Event(){ Id=1, ExternalId="dasdsad", ImagePath="suggestion_seattle_waterfront.png", Name="test", StartTime=DateTime.Now, Venue=new Venue(){ Latitude=9, Longitude =0 ,Name="西安"} },
                new Event(){ Id=2, ExternalId="dasdsad", ImagePath="suggestion_seattle_waterfront.png", Name="test", StartTime=DateTime.Now, Venue=new Venue(){ Latitude=9, Longitude =0 ,Name="西安"} },
                new Event(){ Id=3, ExternalId="dasdsad", ImagePath="suggestion_seattle_waterfront.png", Name="test", StartTime=DateTime.Now, Venue=new Venue(){ Latitude=9, Longitude =0 ,Name="西安"} },
                new Event(){ Id=4, ExternalId="dasdsad", ImagePath="suggestion_seattle_waterfront.png", Name="test", StartTime=DateTime.Now, Venue=new Venue(){ Latitude=9, Longitude =0 ,Name="西安"} },
                new Event(){ Id=5, ExternalId="dasdsad", ImagePath="suggestion_seattle_waterfront.png", Name="test", StartTime=DateTime.Now, Venue=new Venue(){ Latitude=9, Longitude =0 ,Name="西安"} }
            };
            return events;
        }

        public async Task<Event> GetEventById(int eventId)
        {
            var allEvents = await GetEvents();

            return allEvents.FirstOrDefault(e => e.Id == eventId);
        }
    }
}
