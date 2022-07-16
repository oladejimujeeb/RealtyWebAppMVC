using System;

namespace RealtyWebApp.Entities
{
    public class Payment:BaseEntity
    {
        public string BuyerName { get; set; }
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
        public string BuyerEmail{ get; set; }
        public string TransactionId { get; set; }
        public double TotalPrice { get; set; }
        public double Amount { get; set; }
        public string BuyerTelephone{ get; set; }
        public int PropertyId { get; set; }
        public string PropertyType{ get; set; }
        public Property Property{ get; set; }
        public DateTime PaymentDate { get; set; }
    }

    public enum PaymentStatus
    {
        Pending=0,
        Success=1,
        
    }
}