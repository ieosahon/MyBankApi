using System;

namespace MyBankApi.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AccountName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public decimal AccountBalance { get; set; }
        public AccountType AccountType { get; set; }
        public string AccountNumberGenerated { get; set; }
        
        // pin hash and salt used to encrypt the account transaction pin
        public byte[] PinHash { get; set; }
        public byte[] PinSalt { get; set; }
        public DateTime  DateCreated { get; set; }
        public DateTime DateLastUpdated { get; set; }


    }

    public enum AccountType
    {
        Savings,
        Current,
        Corporate,
        Government
    }
}
