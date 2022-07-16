using System;

namespace RealtyWebApp.Models.RequestModel
{
    public class PaymentRequestModel
    {
        public string BuyerName { get; set; }
        public string BuyerEmail{ get; set; }
        public string TransactionId { get; set; }
        public double TotalPrice { get; set; }
        public double Amount { get; set; }
        public string BuyerTelephone{ get; set; }
        public int PropertyId { get; set; }
        public string PropertyRegNumber { get; set; }
        public string PropertyType{ get; set; }
        public DateTime PaymentDate { get; set; }
    }
}