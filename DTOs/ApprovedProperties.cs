using System;
using System.Collections.Generic;

namespace RealtyWebApp.DTOs
{
    public class ApprovedProperties
    {
        public string PropertyRegNumber { get; set; }
        public string PropertyType { get; set; }
        public string BuildingType { get; set; }
        public string Features { get; set; }
        public string Address { get; set; }
        public double PropertyPrice { get; set; }
        public DateTime RegisteredDate { get; set; }
        public string Status { get; set; }
        public double LandArea { get; set; }

        public string Name { get;set; }
    }
}