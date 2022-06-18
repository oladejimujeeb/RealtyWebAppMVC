using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace RealtyWebApp.Models.RequestModel
{
    public class AdminRequestModel
    {
        [Required]
        [MaxLength(30)]
        public string FirstName { get; set; }
        
        [Required]
        [MaxLength(30)]
        public string LastName { get; set; }
        
        [Required]
        /*[RegularExpression("@ .com")]
        [DataType(DataType.EmailAddress)]*/
        public string Email { get; set; }
        
        [Required]
        [MinLength(7)]
        [MaxLength(15)]
        public string Password{get;set;}
        
        [Required]
        [Compare("Password",ErrorMessage = "Password and the confirm password must be the same")]
        public string ConfirmPassword{get;set;}
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        
        public string Address { get; set; }
        public IFormFile ProfilePicture { get; set; }
    }
}