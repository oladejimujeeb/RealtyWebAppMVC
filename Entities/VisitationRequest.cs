using System;

namespace RealtyWebApp.Entities
{
    public class VisitationRequest:BaseEntity
    {
        public string BuyerName { get; set; }
        public int BuyerId { get; set; }
        public Buyer Buyer{ get; set; }
        public string BuyerEmail{ get; set; }
        public string BuyerTelephone{ get; set; }
        public string PropertyType{ get; set; }
        public int PropertyId { get; set; }
        public Property Property{ get; set; }
        public string PropertyRegNo { get; set; }
        public DateTime? RequestDate { get; set; }
        public string Address { get; set; }
    }
}