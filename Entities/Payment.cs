using System;

namespace RealtyWebApp.Entities
{
    public class Payment:BaseEntity
    {
        public string BuyerName { get; set; }
        /*public int BuyerId { get; set; }
        public Buyer Buyer{ get; set; }*/
        public string BuyerEmail{ get; set; }
        public double Amount { get; set; }
        public string BuyerTelephone{ get; set; }
        public int PropertyId { get; set; }
        public string PropertyType{ get; set; }
        public Property Property{ get; set; }
        public DateTime PaymentDate { get; set; }
    }
}