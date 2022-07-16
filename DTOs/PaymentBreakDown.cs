namespace RealtyWebApp.DTOs
{
    public class PaymentBreakDown
    {
        public int PropertyId { get; set; }
        public double PropertyPrice { get; set; }
        public string BuyerName { get; set; }
        public string BuyerEmail{ get; set; }
        public string TransactionId { get; set; }
        public string PropertyRegNumber{ get; set; }
        public double AgreementFees { get; set; }
        public double AgencyFees { get; set; }
        public double TotalPrice { get; set; }
        public string BuyerTelephone{ get; set; }
        public string PropertyType{ get; set; }
    }
}