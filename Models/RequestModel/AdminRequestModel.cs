using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
namespace RealtyWebApp.Models.RequestModel
{
    public class AdminRequestModel
    {
        [Required]
        [MaxLength(30)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        
        [Required]
        [MaxLength(30)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        
        [Required]
        /*[RegularExpression("@ .com")]
        [DataType(DataType.EmailAddress)]*/
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        [MinLength(7)]
        [MaxLength(15)]
        [DataType(DataType.Password)]
        public string Password{get;set;}
        
        [Required]
        [Display(Name = "Confirm Password")]
        [Compare("Password",ErrorMessage = "Password and the confirm password must be the same")]
        public string ConfirmPassword{get;set;}
        
        [Required]
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        [MaxLength(100)]
        [Required(ErrorMessage = "Address Cannot be empty")]
        public string Address { get; set; }
        
        [Display(Name = "Profile Picture")]
        [DataType(DataType.Upload)]
        [Required(ErrorMessage = "Upload your profile picture")]
        public IFormFile ProfilePicture { get; set; }
    }
}