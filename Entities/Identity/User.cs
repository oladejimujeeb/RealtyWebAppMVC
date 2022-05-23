using System.Collections.Generic;

namespace RealtyWebApp.Entities.Identity
{
    public class User:BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public Admin Admin { get; set; }
        public Buyer Buyer { get; set; }
        public Realtor Realtor { get; set; }
        public string ProfilePicture { get; set; }
        public ICollection<UserRole>UserRoles{ get; set; }=new HashSet<UserRole>();
    }
}