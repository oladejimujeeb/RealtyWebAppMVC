using System.Collections.Generic;
using System.Collections.ObjectModel;
using RealtyWebApp.Entities.Identity;

namespace RealtyWebApp.Entities
{
    public class Realtor:BaseEntity
    {
        public string AgentId { get; set; }
        public string Address { get; set; }
        public string BusinessName { get; set; }
        public string CacRegistrationNumber { get; set; }
        public User User{get;set;}
        public Wallet Wallet { get; set; }
        public int UserId{get;set;}
        public IList<Property> Properties { get; set; } = new List<Property>();
    }
}