using System.ComponentModel.DataAnnotations;

namespace RealtyWebApp.DTOs.PayStack
{
    public class TransferRequest
    {
        [Required(ErrorMessage = "Account Number Cannot be empty")]
        [Display(Name = "Account Number")]
        public string AccountNo { get; set; }
        [Display(Name = "Bank Name")]
        [Required]
        public string BankCode { get; set; }
        [Display(Name = "Account Name")]
        [Required(ErrorMessage = "Account Name Cannot be empty")]
        public string AccountName { get; set; }
        
    }

    public class Withdraw
    {
        [Required]
        public int Amount { get; set; }
        [Required]
        public string Password { get; set; }
    }
}