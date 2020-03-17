using System;
using System.Collections.Generic;
using System.Text;

namespace MyCensus.Models
{
    /// <summary>
    /// 地图标记类
    /// </summary>
    public class Option
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        public double Lat { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        public double Lng { get; set; }

    }
}
