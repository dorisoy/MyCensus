using MyCensus.ViewModels.Base;
using System;
using MyCensus.Models.Census;


namespace MyCensus.Models.Rides
{
    public class Ride : ExtendedBindableObject
    {


        private bool _isSelected;
        public int Id { get; set; }

        public int? EventId { get; set; }

        public RideType RideType { get; set; }

        /// <summary>
        /// 普查任务名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime Start { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime Stop { get; set; }

        /// <summary>
        /// 起始地点
        /// </summary>
        public string From { get; set; }
        /// <summary>
        /// 到达地点
        /// </summary>
        public string To { get; set; }


        /// <summary>
        /// 起始地理位置
        /// </summary>
        public Station FromStation { get; set; }
        /// <summary>
        /// 到达地理位置
        /// </summary>
        public Station ToStation { get; set; }

        /// <summary>
        /// 距离
        /// </summary>
        public int Distance { get; set; }

        /// <summary>
        /// 持续时间
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// 是否选择
        /// </summary>
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                RaisePropertyChanged(() => IsSelected);
            }
        }

        public string City { get; set; }
        /// <summary>
        /// 传统普查卡信息
        /// </summary>
        public Tradition TraditionInfo { get; set; }

        /// <summary>
        /// 餐饮普查卡信息
        /// </summary>
        public Restaurant RestaurantInfo { get; set; }

        /// <summary>
        /// 网点图片缩略
        /// </summary>
        public string ThumbnailPhoto { get; set; }
    }
}