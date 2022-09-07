﻿using MyBankApi.DAL;
using MyBankApi.Models;
using MyBankApi.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using MyBankApi.Validations;
using MyBankApi.Utilities;

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
            if (!Validation.VerifyPinHash(Pin, acc.PinHash, acc.PinSalt)) throw new ApplicationException("Invalid Pin");
            return acc;
        }
        

        public Account Create(Account account, string Pin, string ConfirmPin)
        {
            var myAccount = _bankAppDbContext.Accounts.Any(x => x.Email == account.Email);
            if (myAccount) throw new ApplicationException("Account already exists with this email");

            // check if pin matches
            if (!Pin.Equals(ConfirmPin)) throw new ApplicationException("Pin does not match");

            // hashing/encrypting pin
            byte[] pinHash, pinSalt;
            Utility.CreatePinHash(Pin, out pinHash, out pinSalt);
            account.PinHash = pinHash;
            account.PinSalt = pinSalt;

            // create the new account

            account.DateCreated = DateTime.Now;
            _bankAppDbContext.Accounts.Add(account);
                _bankAppDbContext.SaveChanges();
                return account;
            
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
            var myAccount = _bankAppDbContext.Accounts.Where(x => x.Id == account.Id).SingleOrDefault();
            if (myAccount == null) throw new ApplicationException("Account does not exist");
            if (myAccount != null)
            {
                if (!string.IsNullOrWhiteSpace(Pin))
                {
                    Utility.CreatePinHash(Pin, out byte[] pinHash, out byte[] pinSalt);
                    myAccount.PinHash = pinHash;
                    myAccount.PinSalt = pinSalt;
                }

                // to update phone number
                if (!string.IsNullOrWhiteSpace(account.PhoneNumber))
                {
                    myAccount.PhoneNumber = account.PhoneNumber;
                }
               
            }

            account.DateLastUpdated = DateTime.Now;
            _bankAppDbContext.Accounts.Update(myAccount);
            _bankAppDbContext.SaveChanges();
        }
    }
}
