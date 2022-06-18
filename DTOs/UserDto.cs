namespace RealtyWebApp.DTOs
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string ProfilePicture { get; set; }
        public int RealtorId { get; set; }
        public int BuyerId { get; set; }
        public string RoleName { get; set; }
    }
}