using System;
using System.Collections.Generic;
using System.Text;
using MyCensus.Models;

namespace MyCensus.Controls
{
    public class LocationUpdatedEventArgs : EventArgs
    {
        public Coordinate Coordinate { get; set; }
        public double Direction { get; set; }
        public double Altitude { get; set; }
        public double Accuracy { get; set; }
        public int Satellites { get; set; }
        public string Type { get; set; }

        /// <summary>
        /// 省份
        /// </summary>
        public string Province { get; set; }

        /// <summary>
        /// 市
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 区县
        /// </summary>
        public string District { get; set; }

        /// <summary>
        /// 街道
        /// </summary>
        public string Street { get; set; }

        public string Address { get; set; }
    }

    public class LocationFailedEventArgs : EventArgs
    {
        public string Message { get; }
        public LocationFailedEventArgs(string message)
        {
            Message = message;
        }
    }

    public interface ILocationService
    {
        void Start(List<Option> points);
        void Stop();
        bool IsStarted { get; }
        void Refresh(List<Option> points);
        void AnimateTo(Coordinate point);

        event EventHandler<LocationUpdatedEventArgs> LocationUpdated;
        event EventHandler<LocationFailedEventArgs> Failed;
    }
}
