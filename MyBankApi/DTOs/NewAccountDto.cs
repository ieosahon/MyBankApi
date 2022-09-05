using MyBankApi.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace MyBankApi.DTOs
{
    public class NewAccountDto
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        //public string AccountName { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        //public decimal AccountBalance { get; set; }
        [Required]
        public AccountType AccountType { get; set; }
        //public string AccountNumberGenerated { get; set; }

        // pin hash and salt used to encrypt the account transaction pin
        //public byte[] PinHash { get; set; }
        //public byte[] PinSalt { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateLastUpdated { get; set; }
        [Required]
        [RegularExpression(@"^[0-9]/d{4}$*", ErrorMessage ="Pin should not be less or more than 4 digits")] // the pin should be a 4 digits
        public string Pin { get; set; }
        [Compare("Pin", ErrorMessage ="Pins do not match!")]
        public string ConfirmPin { get; set; }
    }
}
