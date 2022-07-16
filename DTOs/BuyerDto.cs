namespace RealtyWebApp.DTOs
{
    public class BuyerDto
    {
        public int Id { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string RegNo { get; set; }
        public string Address { get; set; }
        public string Role { get; set; }
        public string ProfilePicture { get; set; }
    }

    public class UpdateBuyerModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
    }
}