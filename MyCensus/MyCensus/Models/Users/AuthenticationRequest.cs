namespace MyCensus.Models.Users
{
    /// <summary>
    /// 身份认证请求
    /// </summary>
    public class AuthenticationRequest
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 资质
        /// </summary>
        public string Credentials { get; set; }

        /// <summary>
        /// 授权类型
        /// </summary>
        public string GrantType { get; set; }
    }
}
