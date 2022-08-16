namespace RealtyWebApp.DTOs.PayStack
{
    public class Transfer
    {
        public string status { get; set; }
        public string message { get; set; }
        public TransferData data { get; set; }
    }

    public class TransferData
    {
       public string reference { get; set; }
       public string integration { get; set; }
       public string domain { get; set; }
       public double amount { get; set; }
       public string currency { get; set; }
       public string source { get; set; }
       public string reason { get; set; }
       public string recipient { get; set; }
       public string status { get; set; }
       public string tranfer_code { get; set; }
       public int id { get; set; }
       public string createdAt { get; set; }
       public string updatedAt { get; set; }
    }
}