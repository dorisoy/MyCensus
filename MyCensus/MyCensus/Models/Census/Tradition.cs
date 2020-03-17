using System;
using System.Collections.Generic;
using System.Text;


namespace MyCensus.Models.Census
{

    public class RandomObject
    {
        public string RandomProperty1 { get; set; }
    }


    /// <summary>
    /// 销售商品
    /// </summary>
    public class SalesProduct : EntityBase
    {
        public SalesProduct()
        {
            GuId = Guid.NewGuid().ToString();
        }

        #region 销售信息

        public string GuId { get; set; }

        /// <summary>
        /// 品牌(终端所销售的啤酒品牌)青岛啤酒、雪花啤酒、Budweiser百威、其他
        /// </summary>
        public string Brand { get; set; }
        /// <summary>
        ///  产品名称(扫码+手输)
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        ///  年销量（填写整数 年/箱）4大类品牌（青岛啤酒、雪花啤酒、Budweiser百威、其他）分为四档的容量，高档及以上，中档及中档高，中档低、其他
        /// </summary>
        public string AnnualSales { get; set; }

        /// <summary>
        ///  包装形式(瓶、听、桶)(所售啤酒产品的最小包装形式)
        /// </summary>
        public string PackingForm { get; set; }

        /// <summary>
        /// 产品供货商
        /// </summary>
        public string ProductProvider { get; set; }

        /// <summary>
        /// 渠道属性((一批、二批、其他)（销售啤酒产品给此终端的渠道商在其代理的啤酒产品渠道中的层级，包括一批、二批和其他（一批、二批之外））
        /// </summary>
        public string ChannelAttributes { get; set; }

        /// <summary>
        /// 规格
        /// </summary>
        public string Specification { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string BarCode { get; set; }


        /// <summary>
        /// 档次
        /// </summary>
        public string Grade { get; set; }

        #endregion
    }


    public class EntityBase
    {
        public int Id { get; set; }
    }

    /// <summary>
    /// 传统普查卡信息
    /// </summary>
    public class Tradition : EntityBase
    {
        public Tradition()
        {
            SalesProducts = new List<SalesProduct>();
            DoorheadPhotos = new List<DoorheadPhoto>();
            DisplayPhotos = new List<DisplayPhoto>();
            TrackLocation = new TrackLocation();
        }

        /// <summary>
        /// 用户标识
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 纬度坐标
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// 经度坐标
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// 地图标注位置
        /// </summary>
        public string Location { get; set; }

        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 基本信息
        /// </summary>
        public TraditionBaseInfo BaseInfo { get; set; }

        /// <summary>
        /// 营业信息
        /// </summary>
        public TraditionBusinessInfo BusinessInfo { get; set; }


        public List<SalesProduct> SalesProducts { get; set; }

        public List<DoorheadPhoto> DoorheadPhotos { get; set; }

        public List<DisplayPhoto> DisplayPhotos { get; set; }


        public TrackLocation TrackLocation { get; set; }

    }

    /// <summary>
    /// 基本信息
    /// </summary>
    public class TraditionBaseInfo
    {

        /// <summary>
        /// 终端编号
        /// </summary>
        public string EndPointNumber { get; set; }
        /// <summary>
        /// 大区
        /// </summary>
        public string SaleRegion { get; set; }
        /// <summary>
        /// 业务部
        /// </summary>
        public string SalesDepartment { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 区/县
        /// </summary>
        public string DistrictOrCounty { get; set; }
        /// <summary>
        /// 城区/乡镇
        /// </summary>
        public string CityOrTown { get; set; }
        /// <summary>
        /// 终端店名
        /// </summary>
        public string EndPointStorsName { get; set; }
        /// <summary>
        /// 营业电话
        /// </summary>
        public string EndPointTelphone { get; set; }

        /// <summary>
        /// 终端地址(（详细街道/门牌号）)
        /// </summary>
        public string EndPointAddress { get; set; }


    }

    /// <summary>
    /// 营业信息
    /// </summary>
    public class TraditionBusinessInfo 
    {

        /// <summary>
        /// 终端类型(现代小零售(X)/名烟名酒店(M)/传统小零售(T)/批零店（P）)
        /// </summary>
        public string EndPointType { get; set; }

        /// <summary>
        /// 是否连锁
        /// </summary>
        public bool IsChain { get; set; }

        /// <summary>
        /// 是否协议店
        /// </summary>
        public bool IsAgreement { get; set; }

        /// <summary>
        /// 合作方式(专营保量、专营不保量、混营保量、混营不保量)是协议店时必填，非协议店不填
        /// </summary>
        public string ModeOfCooperation { get; set; }

        /// <summary>
        // 终端状态(专销、强势混销、弱势混销、空白)
        //专销：单店我司份额=100%;强势混销：单店我司份额第一且>=40%；弱势混销：单店我司份额<40% or 第二名及以后；空白：单店我司份额=0
        /// </summary>
        public string EndPointStates { get; set; }

        /// <summary>
        /// 负责人姓名
        /// </summary>
        public string ChargePerson { get; set; }

        /// <summary>
        /// 电话(终端负责人的电话)
        /// </summary>
        public string Telphone { get; set; }

    }

    /// <summary>
    /// 餐饮普查卡信息
    /// </summary>
    public class Restaurant : EntityBase
    {

        public Restaurant()
        {
            SalesProducts = new List<SalesProduct>();
            DoorheadPhotos = new List<DoorheadPhoto>();
            DisplayPhotos = new List<DisplayPhoto>();
            TrackLocation = new TrackLocation();
        }



        /// <summary>
        /// 用户标识
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 纬度坐标
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// 经度坐标
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// 地图标注位置
        /// </summary>
        public string Location { get; set; }

        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 基本信息
        /// </summary>
        public RestaurantBaseInfo BaseInfo { get; set; }

        /// <summary>
        /// 营业信息
        /// </summary>
        public RestaurantBusinessInfo BusinessInfo { get; set; }


        public List<SalesProduct> SalesProducts { get; set; }

        public List<DoorheadPhoto> DoorheadPhotos { get; set; }

        public List<DisplayPhoto> DisplayPhotos { get; set; }

        public TrackLocation TrackLocation { get; set; }
    }

    /// <summary>
    /// 基本信息
    /// </summary>
    public class RestaurantBaseInfo 
    {


        /// <summary>
        /// 终端编号
        /// </summary>
        public string EndPointNumber { get; set; }
        /// <summary>
        /// 大区
        /// </summary>
        public string SaleRegion { get; set; }
        /// <summary>
        /// 业务部
        /// </summary>
        public string SalesDepartment { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 区/县
        /// </summary>
        public string DistrictOrCounty { get; set; }
        /// <summary>
        /// 城区/乡镇
        /// </summary>
        public string CityOrTown { get; set; }
        /// <summary>
        /// 终端店名
        /// </summary>
        public string EndPointStorsName { get; set; }
        /// <summary>
        /// 营业电话
        /// </summary>
        public string EndPointTelphone { get; set; }

        /// <summary>
        /// 终端地址(（详细街道/门牌号）)
        /// </summary>
        public string EndPointAddress { get; set; }

    }

    /// <summary>
    /// 营业信息
    /// </summary>
    public class RestaurantBusinessInfo
    {
        /// <summary>
        /// 终端类型(S超高档餐饮、A高档餐饮、B中档餐饮、C大众餐饮、D低档餐饮)
        /// </summary>
        public string EndPointType { get; set; }

        /// <summary>
        /// 是否连锁
        /// </summary>
        public bool IsChain { get; set; }

        /// <summary>
        /// 包厢数(终端中的包厢个数)
        /// </summary>
        public int PrivateRoomes { get; set; }

        /// <summary>
        /// 桌台数
        /// </summary>
        public int TableNumber { get; set; }

        /// <summary>
        /// 座位数
        /// </summary>
        public int Seats { get; set; }

        /// <summary>
        /// 经营特色(炒菜/火锅/烧烤/小吃（面馆）快餐/西餐（含日韩料理/自助餐/其它)
        /// </summary>
        public string Characteristics { get; set; }

        /// <summary>
        /// 是否协议店
        /// </summary>
        public bool IsAgreement { get; set; }

        /// <summary>
        /// 合作方式(专营保量、专营不保量、混营保量、混营不保量)
        /// </summary>
        public string ModeOfCooperation { get; set; }

        /// <summary>
        /// 终端状态(专销、强势混销、弱势混销、空白)
        /// </summary>
        public string EndPointStates { get; set; }


        public string PerConsumptions { get; set; }

        /// <summary>
        /// 负责人姓名
        /// </summary>
        public string ChargePerson { get; set; }

        /// <summary>
        /// 电话(终端负责人的电话)
        /// </summary>
        public string Telphone { get; set; }

        /// <summary>
        /// 终端关键人1姓名
        /// </summary>
        public string TopContacts { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string TopContactPhone { get; set; }

        /// <summary>
        /// 职位
        /// </summary>
        public string Position { get; set; }

        /// <summary>
        /// 本品冰展柜投入数量
        /// </summary>
        public int ShowcaseNum { get; set; }


    }



    /// <summary>
    /// 门头照片
    /// </summary>
    public class DoorheadPhoto: EntityBase
    {
        public string StoragePath { get; set; }
    }

    /// <summary>
    /// 陈列照片
    /// </summary>
    public class DisplayPhoto : EntityBase
    {
        public string DisplayPath { get; set; }
    }



    public class Statistics
    {
        public string Type { get; set; }
        public int EndPointSum { get; set; }
    }


    public class TrackLocation : EntityBase
    {
        /// <summary>
        /// 用户标识
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 终端编号
        /// </summary>
        public string EndPointNumber { get; set; }

        public int? EventId { get; set; }

        /// <summary>
        /// 普查类型（传统0，餐饮1，渠道2，KA3）
        /// </summary>
        public int CensusType { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime Start { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime Stop { get; set; }

        /// <summary>
        /// 持续时间
        /// </summary>
        public int Duration { get; set; }
    }


}
