using System;
using System.ComponentModel.DataAnnotations;

namespace MyBankApi.DTOs
{
    public class AccountUpdateDto
    {
        [Key]
        public int Id { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }    
        public DateTime DateLastUpdated { get; set; }
        [RegularExpression(@"^[0-9]/d{4}$*", ErrorMessage = "Pin should not be less or more than 4 digits")] // the pin should be a 4 digits
        public string Pin { get; set; }
        [Compare("Pin", ErrorMessage = "Pins do not match!")]
        public string ConfirmPin { get; set; }
    }
}
