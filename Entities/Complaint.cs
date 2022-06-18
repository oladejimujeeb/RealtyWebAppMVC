using System;

namespace RealtyWebApp.Entities
{
    public class Complaint:BaseEntity
    {
        public string Title { get; set; }
        public string ComplainMessage { get; set; }
        public DateTime ComplaintDate { get; set; }
    }
}