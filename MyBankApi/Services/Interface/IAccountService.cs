using MyBankApi.Models;
using System.Collections.Generic;

namespace MyBankApi.Services.Interface
{
    public interface IAccountService
    {
        Account Authenticate(string AccountNumber, string Pin);
        IEnumerable<Account> GetAllAccounts();
        Account Create(Account account, string Pin, string ConfirmPin);
        void Update(Account account, string Pin = null);
        void Delete(Account account);
        Account GetAccountById(int Id);
        Account GetAccountByNumber(string AccountNumber);
    }
}
