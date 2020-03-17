namespace MyCensus.Models.Rides
{
    /// <summary>
    /// 位置
    /// </summary>
    public class Station
    {
        public int Id { get; set; }

        /// <summary>
        /// 位置名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        public float Latitude { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public float Longitude { get; set; }

        /// <summary>
        /// 当前时段
        /// </summary>
        public int Slots { get; set; }

        /// <summary>
        /// 已占用的，在使用的
        /// </summary>
        public int Occupied { get; set; }

        /// <summary>
        /// 停靠时间
        /// </summary>
        public int EmptyDocks => Slots - Occupied;
    }
}
