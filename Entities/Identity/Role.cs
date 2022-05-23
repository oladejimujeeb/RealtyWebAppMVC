using System.Collections.Generic;

namespace RealtyWebApp.Entities.Identity
{
    public class Role:BaseEntity
    {
        public string RoleName { get; set; }
        public ICollection<UserRole>UserRoles{ get; set; }= new HashSet<UserRole>();
    }
}