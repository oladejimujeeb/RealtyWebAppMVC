using System.Collections.Generic;

namespace RealtyWebApp.DTOs.PayStack
{
    public class PayStackResponse
    {
        public bool status { get; set; }
        public string message { get; set; }
        public Data data { get; set; }
    }

    public class Data
    {
        public string authorization_url { get; set; }
        public string access_code { get; set; }
        public string reference { get; set; }
        public string status { get; set; }
    }
}