using RealtyWebApp.Entities.Identity;

namespace RealtyWebApp.Entities
{
    public class Admin:BaseEntity
    {
        public string RegId { get; set; }
        public string Address { get; set; }
        public User User{get;set;}
        public int UserId{get;set;}
    }
}