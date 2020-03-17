using System;

namespace MyCensus.Models
{
    public class Item : BaseDataObject
    {
        string text = string.Empty;
        public string Text
        {
            get { return text; }
            set { SetProperty(ref text, value); }
        }

        string description = string.Empty;
        public string Description
        {
            get { return description; }
            set { SetProperty(ref description, value); }
        }
    }


    public class TrackTimeLine 
    {
        public DateTime UpdateTime { get; set; }
        public string CensusType { get; set; }
        public string EndPointStorsName { get; set; }
        public string EndPointAddress { get; set; }
        public bool IsLast { get; set; } = false;

        /// <summary>
        /// 纬度坐标
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// 经度坐标
        /// </summary>
        public double Longitude { get; set; }

        public string Duration { get; set; }
    }


    public class DropdownClass
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public override string ToString() => Name;
    }

    /// <summary>
    /// 普查卡信息实体
    /// </summary>
    public class CardInfo 
    {
        public string EndPointNumber { get; set; }
        public string Region { get; set; }
        public string CardType { get; set; }
        public string CreateTime { get; set; }
    }


    public class Stats
    {
        public string Title { get; set; }
        public string Value1 { get; set; }
        public string Label1 { get; set; }
        public string Value2 { get; set; }
        public string Label2 { get; set; }
    }

    public class UploadResult
    {
        public string Id { get; set; }
        public string Success { get; set; }
        public string MD5 { get; set; }
    }
}
