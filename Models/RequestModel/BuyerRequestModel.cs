using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace RealtyWebApp.Models.RequestModel
{
    public class BuyerRequestModel
    {
        [Required(ErrorMessage = "Enter your first name")]
        [MaxLength(30)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        
        [Required (ErrorMessage = "Enter your last name")]
        [MaxLength(30)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        
        [Required(ErrorMessage = "Email cannot be empty")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        
        [Required]
        [MinLength(7)]
        [MaxLength(15)]
        [DataType(DataType.Password)]
        public string Password{get;set;}
        
        [Required]
        [MinLength(7)]
        [MaxLength(15)]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage = "Password and the confirm password must be the same")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword{get;set;}
        
        [Required (ErrorMessage = "Phone number cannot be empty")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
          
        [Required(ErrorMessage = "Address cannot be empty")]
        [DataType(DataType.Text)]
        public string Address { get; set; }
        [Required(ErrorMessage = "Upload your profile picture")]
        [DataType(DataType.Upload)]
        [Display(Name = "Upload Profile Picture")]
        public IFormFile ProfilePicture { get; set; }
    }
}