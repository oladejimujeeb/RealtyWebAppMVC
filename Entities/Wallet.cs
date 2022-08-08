namespace RealtyWebApp.Entities
{
    public class Wallet:BaseEntity
    {
        public double AccountBalance { get; set; } = 0.00;
        public string ReceipientCode { get; set; }
        public string AccountName { get; set; }
        public string BankName { get; set; }
        public  string AccountNo { get; set; }
        public Realtor Realtor { get; set; }
        public int RealtorId { get; set; }
    }
}