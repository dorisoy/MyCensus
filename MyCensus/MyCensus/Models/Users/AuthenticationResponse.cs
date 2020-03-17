using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace MyCensus.Models.Users
{

    /// <summary>
    /// 返回消息类
    /// </summary>
    public class AuthenticationResponse
    {
        public int Status { get; set; }
        public string Msg { get; set; }
        public JH_Auth_User User { get; set; }
        public string AccessToken { get; set; }
    }


    /// <summary>
    /// 返回消息类
    /// </summary>
    public class RequestResult 
    {
        public int status { get; set; }
        public string msg { get; set; }
    }

    public class JH_Auth_User 
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string UserRealName { get; set; }
        public int BranchCode { get; set; }
        public string Sex { get; set; }
        public string IsUse { get; set; }
        public string STATUS { get; set; }
        public string Usersign { get; set; }
        public string telphone { get; set; }
        public string mobphone { get; set; }
        public string tx { get; set; }
        public string zhiwu { get; set; }
        public string remark { get; set; }
        public Nullable<int> remark1 { get; set; }
        public string mailbox { get; set; }
        public string weixinnum { get; set; }
        public string pccode { get; set; }
        public Nullable<System.DateTime> pccodeupdate { get; set; }
        public string mobcode { get; set; }
        public Nullable<System.DateTime> mobcodedate { get; set; }
        public Nullable<int> ComId { get; set; }
        public string UserLeader { get; set; }
        public Nullable<int> UserLogoId { get; set; }
        public Nullable<int> UserOrder { get; set; }
        public string txurl { get; set; }
        public string isgz { get; set; }
        public Nullable<System.DateTime> logindate { get; set; }
        public string TXLQX { get; set; }
        public string RoleQX { get; set; }
        public string RoomCode { get; set; }
        public string UserGW { get; set; }
        public string NickName { get; set; }
        public string JobNum { get; set; }
        public string QQ { get; set; }
        public string Nationality { get; set; }
        public string Nation { get; set; }
        public string HomeAddress { get; set; }
        public string GSPhone { get; set; }
        public Nullable<System.DateTime> EntryDate { get; set; }
        public Nullable<System.DateTime> DepartureDate { get; set; }
        public Nullable<System.DateTime> Birthday { get; set; }
        public string Marriage { get; set; }
        public string Children { get; set; }
        public string JJLXR { get; set; }
        public string JJLXRDH { get; set; }
        public string Hire { get; set; }
        public Nullable<System.DateTime> ZZTime { get; set; }
        public Nullable<System.DateTime> ContractSTime { get; set; }
        public Nullable<System.DateTime> ContractETime { get; set; }
        public string CRUser { get; set; }
        public Nullable<System.DateTime> CRDate { get; set; }
        public string weixinCard { get; set; }

        public string Files { get; set; }
        public string RegionCode { get; set; }
        public string GLevel { get; set; }
        public string MinRegion { get; set; }
        public string Zhineng { get; set; }
        public string SalesDepartment { get; set; }

    }
}
