using System;

namespace MyCensus.Models.Events
{
    public class Event
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ImagePath { get; set; }

        public DateTime StartTime { get; set; }

        public string ExternalId { get; set; }

        /// <summary>
        /// 网点
        /// </summary>
        public Venue Venue { get; set; }
    }
}
