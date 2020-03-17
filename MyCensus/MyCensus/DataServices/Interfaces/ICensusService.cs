using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using MyCensus.Models.Census;
using MyCensus.Models;
using MyCensus.Models.Events;
using MyCensus.Models.Rides;
using MyCensus.Models.Users;

namespace MyCensus.DataServices.Interfaces
{
    public interface ICensusService
    {
        Task<List<Tradition>> GetTraditions(int limit, int pagenumber);
        Task<List<Restaurant>> GetRestaurants(int limit, int pagenumber);

        Task<RequestResult> AddTradition(Tradition tradition);
        Task<RequestResult> AddRestaurant(Restaurant restaurant);

        Task<Tradition> GetTradition(int id);
        Task<Restaurant> GetRestaurant(int id);

        Task<List<Statistics>> GetStatistics();

        Task<List<TrackTimeLine>> GetTrackTimeLines();

        Task<RequestResult> AddTrackLocation(TrackLocation trackLocation);
    }
}
