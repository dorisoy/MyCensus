using System.Collections.Generic;

namespace MyCensus.Models.Users
{
    public class Tenant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<JH_Auth_User> Users { get; set; }
    }
}
