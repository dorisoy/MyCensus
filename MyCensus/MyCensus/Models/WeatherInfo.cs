namespace MyCensus.Models
{
    public class WeatherInfo : IWeatherResponse
    {
        public float Temp { get; set; }
        public TempUnit TempUnit { get; set; }
        public string LocationName { get; set; }
    }
}