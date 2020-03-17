using MyCensus.Controls.Cells;
using MyCensus.Models.Rides;
using Xamarin.Forms;

namespace MyCensus.TemplateSelectors
{
    public class MyRidesDataTemplateSelector : DataTemplateSelector
    {
        private readonly DataTemplate _eventDataTemplate;
        private readonly DataTemplate _customRideDataTemplate;
        private readonly DataTemplate _suggestedRideDataTemplate;

        private readonly DataTemplate _traditionRideDataTemplate;
        private readonly DataTemplate _restaurantRideDataTemplate;

        public MyRidesDataTemplateSelector()
        {
            _eventDataTemplate = new DataTemplate(typeof(EventRideCell));
            _customRideDataTemplate = new DataTemplate(typeof(CustomRideCell));
            _suggestedRideDataTemplate = new DataTemplate(typeof(SuggestedRideCell));

            _traditionRideDataTemplate = new DataTemplate(typeof(TraditionRideCell));
            _restaurantRideDataTemplate = new DataTemplate(typeof(RestaurantRideCell));
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var ride = item as Ride;

            if (ride == null)
            {
                return null;
            }

            switch (ride.RideType)
            {
                case RideType.Event:
                    return _eventDataTemplate;
                case RideType.Suggestion:
                    return _suggestedRideDataTemplate;
                case RideType.Custom:
                    return _customRideDataTemplate;
                case RideType.Tradition:
                    return _traditionRideDataTemplate;
                case RideType.Restaurant:
                    return _restaurantRideDataTemplate;
                default:
                    return null;
            }
        }
    }
}
