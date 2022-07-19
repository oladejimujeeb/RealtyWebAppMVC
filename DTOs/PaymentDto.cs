using System;

namespace RealtyWebApp.DTOs
{
    public class PaymentDto
    {
        public int Id { get; set; }
        public double PropertyPrice { get; set; }
        public string BuyerName { get; set; }
        public string BuyerEmail{ get; set; }
        public string PropertyRegNum { get; set; }
        public string TransactionId { get; set; }
        public string AgentId{ get; set; }
        public double TotalPrice { get; set; }
        public string BuyerTelephone{ get; set; }
        public string PropertyType{ get; set; }
        public string PaymentDate { get; set; }
        public string AgentEmail { get; set; }
    }
}