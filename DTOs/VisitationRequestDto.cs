using System;

namespace RealtyWebApp.DTOs
{
    public class VisitationRequestDto
    {
        public int Id { get; set; }
        public int BuyerId { get; set; }
        public string BuyerName { get; set; }
        public string BuyerPhoneNo { get; set; }
        public string BuyerEmail { get; set; }
        public int PropertyId { get; set; }
        public string PropertyAddress { get; set; }
        public string PropertyType { get; set; }
        public string PropertyRegNo { get; set; }
        public  string Message { get; set; }
        public  string Mail { get; set; }
        public double PropertyPrice { get; set; }
        public DateTime? RequestDate { get; set; }
        
    }
}