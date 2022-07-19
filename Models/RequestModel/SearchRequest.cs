namespace RealtyWebApp.Models.RequestModel
{
    public class SearchRequest
    {
        public string PropertyRegNo { get;set; }
        public string Location { get; set; }
        public double Price { get; set; }
        public string KeyWord { get; set; }
        public string PropertyType { get; set; }
    }
}