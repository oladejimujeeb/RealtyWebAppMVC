using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System;

namespace RealtyWebApp.Models.RequestModel
{
    public class PropertyRequestModel
    {
        public string PropertyType { get; set; }
        public double Price { get; set; }
        public double PlotArea { get; set; }
        public int Toilet { get; set; }
        public int Bedroom { get; set; }
        public string Features { get; set; }
        public string Address { get; set; }
        public string BuildingType { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string FileDescription { get; set; }
        public string LGA { get; set; }
        public string State { get; set; }
        public List<IFormFile> Files { get; set; } = new List<IFormFile>();
        public List<IFormFile> Images { get; set; } = new List<IFormFile>();
        //public DateTime? RegisteredDate { get; set; }
    }
}