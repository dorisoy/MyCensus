namespace MyCensus.Models.Events
{

    /// <summary>
    /// 终端网点
    /// </summary>
    public class Venue
    {
        /// <summary>
        /// 标识
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        public float Latitude { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        public float Longitude { get; set; }

        /// <summary>
        /// 网点名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 外部标识
        /// </summary>
        public string ExternalId { get; set; }
    }
}