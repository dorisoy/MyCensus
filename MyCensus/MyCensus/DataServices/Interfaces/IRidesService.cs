using MyCensus.Models;
using MyCensus.Models.Events;
using MyCensus.Models.Rides;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyCensus.DataServices.Interfaces
{
    public interface IRidesService
    {
        Task<IEnumerable<Suggestion>> GetSuggestions();

        Task<IEnumerable<Ride>> GetUserRides();

        Task<Station> GetNearestStationTo(GeoLocation location);

        Task<IEnumerable<Station>> GetNearestStations();

        Task<Station> GetInfoForNearestStation();

        Task<Station> GetInfoForNearestStationTo(GeoLocation toGeoLocation);

        Task<Station> GetStation(int stationId);

        Task<Booking> RequestBikeBooking(Station fromStation, Station toStation, Event @event);

        Task<Booking> RequestBikeBooking(Station fromStation, Station toStation);

        Task<IEnumerable<Station>> GetNearestStationsTo(GeoLocation location);

        Task<Booking> GetBooking(int bookingId);

        void RemoveCurrentBooking();
    }
}
