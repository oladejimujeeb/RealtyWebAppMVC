namespace RealtyWebApp.Entities.Identity
{
    public class UserRole:BaseEntity
    {
        public int RoleId{ get; set; }
        public Role Role{ get; set; }
        public int UserId{ get; set; }
        public User User{ get; set; }
    }
}