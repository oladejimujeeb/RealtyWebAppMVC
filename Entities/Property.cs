using System;
using System.Collections.Generic;
using RealtyWebApp.Entities.File;


namespace RealtyWebApp.Entities
{
    public class Property:BaseEntity
    {
        public string PropertyType { get; set; }
        public string Status { get; set; }
        public string Action { get; set; }
        public bool IsAvailable { get; set; }
        public bool VerificationStatus { get; set; }
        public double Price { get; set; }
        public double PlotArea { get; set; }
        public int Toilet { get; set; }
        public int Bedroom { get; set; }
        public string Features { get; set; }
        public string Address { get; set; }
        public string BuildingType { get; set; }
        public double Longitude { get; set; }
        public string PropertyRegNo { get; set; }
        public double Latitude { get; set; }
        public List<PropertyDocument> PropertyDocuments { get; set; } = new List<PropertyDocument>();
        public List<PropertyImage> PropertyImages { get; set; } = new List<PropertyImage>();
        public int BuyerId { get; set; }
        public Buyer Buyer { get; set; }
        /*public FileModel FileModel { get; set; }*/
        public int RealtorId { get; set; }
        public Realtor Realtor { get; set; }
        public Payment Payment { get; set; }
        public ICollection<VisitationRequest> VisitationRequests{ get; set; }= new List<VisitationRequest>();
    }
}