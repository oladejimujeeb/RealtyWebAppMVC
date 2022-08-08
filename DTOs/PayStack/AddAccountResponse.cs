namespace RealtyWebApp.DTOs.PayStack
{
    public class AddAccountResponse
    {
        public bool status { get; set; }
        public string message { get; set; }
        public Response data { get; set; }
    }

    public class Response
    {
        public string active { get; set; }
        public string createdAt { get; set; }
        public string currency { get; set; }
        public string domain{ get; set; }
        public string id{ get; set; }
        public string integration{ get; set; }
        public string name { get; set; }
        public string recipient_code { get; set; }
        public string type { get; set; }
        public string updatedAt { get; set; }
        public bool is_deleted { get; set; }
        public Details details { get; set; }
    }

    public class Details
    {
        public string authorization_code { get; set; }
        public string account_number { get; set; }
        public string account_name { get; set; }
        public string bank_code { get; set; }
        public string bank_name { get; set; }
    }
}