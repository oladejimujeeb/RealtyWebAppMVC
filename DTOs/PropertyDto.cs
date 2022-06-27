using System;
using System.Collections.Generic;
using RealtyWebApp.Entities.Identity.Enum;

namespace RealtyWebApp.DTOs
{
    public class PropertyDto
    {
        public int Id { get; set; }
        public string PropertyType { get; set; }
        public string Status { get; set; }
        public string Action { get; set; }
        public bool IsAvailable { get; set; }
        public bool VerificationStatus { get; set; }
        public double PropertyPrice { get; set; }
        //public string ProPrice { get; set; }
        public double LandArea { get; set; }
        public int Toilet { get; set; }
        public bool IsSold { get; set; }
        public int Bedroom { get; set; }
        public string Features { get; set; }
        public string Address { get; set; }
        public string BuildingType { get; set; }
        public double Longitude { get; set; }
        public string PropertyRegNumber { get; set; }
        public double Latitude { get; set; }
        public int BuyerId { get; set; }
        public int RealtorId { get; set; }
        public IList<string> ImagePath { get; set; } = new List<string>();
        public string PropertyRegNo { get; set; }
        public DateTime? RegisteredDate { get; set; }
        public string LGA { get; set; }
        public string State { get; set; }
        public DateTime SoldDate { get; set; }
    }

    public class UpdatePropertyModel
    {
        public string PropertyType { get; set; }
        public string Status { get; set; }
        public string Action { get; set; }
        /*public bool IsAvailable { get; set; }
        public bool VerificationStatus { get; set; }
        public bool IsSold { get; set; }*/
        public double Price { get; set; }
        public double PlotArea { get; set; }
        public int Toilet { get; set; }
        public int Bedroom { get; set; }
        public string Features { get; set; }
        public string Address { get; set; }
        public string BuildingType { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string LGA { get; set; }
        public string State { get; set; }
        
        
    }
}