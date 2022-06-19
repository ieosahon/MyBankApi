using MyBankApi.DAL;
using MyBankApi.Models;
using MyBankApi.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace MyBankApi.Services.Implementation
{
    public class AccountService : IAccountService
    {
        private readonly BankAppDbContext _bankAppDbContext;
        public AccountService(BankAppDbContext bankAppDbContext)
        {
            _bankAppDbContext = bankAppDbContext;
        }

        public Account Authenticate(string AccountNumber, string Pin)
        {
            // check if account exists
            var acc = _bankAppDbContext.Accounts.Where(x => x.AccountNumberGenerated == AccountNumber).SingleOrDefault();
            if (acc == null) throw new ApplicationException("Account does not exist");

            // Verify PinHash
            if (!VerifyPinHash(Pin, acc.PinHash, acc.PinSalt)) throw new ApplicationException("Invalid Pin");
            return acc;
        }

        // move to a different class later
        private static bool VerifyPinHash(string pin, byte[] pinHash, byte[] pinSalt)
        {
            if (string.IsNullOrEmpty(pin)) throw new ArgumentNullException(nameof(pin));
           
            // pin verification
            using (var hmac = new HMACSHA512(pinSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(pin));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != pinHash[i]) return false;
                }
            }
            return true;
        }

        public Account Create(Account account, string Pin, string ConfirmPin)
        {
            var myAccount = _bankAppDbContext.Accounts.Any(x => x.Email == account.Email);
            if (myAccount) throw new ApplicationException("Account already exists with this email");

            // check if pin matches
            if (!Pin.Equals(ConfirmPin)) throw new ApplicationException("Pin does not match");

            // hashing/encrypting pin
            byte[] pinHash, pinSalt;
            CreatePinHash(Pin, out pinHash, out pinSalt);
            account.PinHash = pinHash;
            account.PinSalt = pinSalt;
            
            // create the new account
            _bankAppDbContext.Accounts.Add(account);
                _bankAppDbContext.SaveChanges();
                return account;
            
        }
        // move to a different class later
        private static void CreatePinHash(string pin, out byte[] pinHash, out byte[] pinSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                pinSalt = hmac.Key;
                pinHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(pin));
            }
        }

        public void Delete(int Id)
        {
            var account = _bankAppDbContext.Accounts.Find(Id);
            if (account == null) throw new ApplicationException("Account does not exist");
            _bankAppDbContext.Accounts.Remove(account);
            _bankAppDbContext.SaveChanges();
        }

        public Account GetAccountById(int Id)
        {
            var account = _bankAppDbContext.Accounts.Where(a => a.Id == Id).FirstOrDefault();
            if (account == null) throw new ApplicationException("Account does not exist");
            return account;
        }

        public IEnumerable<Account> GetAllAccounts()
        {
            var accounts = _bankAppDbContext.Accounts.ToList();
            return accounts;
        }

        public Account GetByAccountNumber(string AccountNumber)
        {
            var account = _bankAppDbContext.Accounts.Where(x => x.AccountNumberGenerated == AccountNumber).FirstOrDefault();
            if (account == null) throw new ApplicationException("Account does not exist");
            return account;
        }

        public void Update(Account account, string Pin = null)
        {
            var myAccount = _bankAppDbContext.Accounts.Where(x => x.AccountNumberGenerated == account.AccountNumberGenerated).SingleOrDefault();
            if (myAccount == null) throw new ApplicationException("Account does not exist");
            if (myAccount != null)
            {
                if (Pin != null)
                {
                    byte[] pinHash, pinSalt;
                    CreatePinHash(Pin, out pinHash, out pinSalt);
                    myAccount.PinHash = pinHash;
                    myAccount.PinSalt = pinSalt;
                }
                _bankAppDbContext.Accounts.Update(myAccount);
                _bankAppDbContext.SaveChanges();
            }
        }
    }
}
