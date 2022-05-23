using System.Collections.Generic;
using RealtyWebApp.Entities.Identity;


namespace RealtyWebApp.Entities
{
    public class Buyer : BaseEntity
    {
        public string RegId { get; set; }
        public string Address { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public IList<Property> Properties { get; set; } = new List<Property>();
        public IList<VisitationRequest> VisitationRequests { get; set; } = new List<VisitationRequest>();
    }

}