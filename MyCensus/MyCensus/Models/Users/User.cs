
namespace MyCensus.Models.Users
{
    /// <summary>
    /// 用户实体
    /// </summary>
    public class User 
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string LastLogin { get; set; }
        public JH_Auth_User Profile { get; set; }
        public System.Collections.Generic.List<Subscription> Subscriptions { get; set; }
        public int TenantId { get; set; }
        public Tenant Tenant { get; set; }
    }
}