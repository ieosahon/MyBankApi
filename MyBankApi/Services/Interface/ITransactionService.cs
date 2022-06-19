using MyBankApi.Models;
using System;
using System.Collections.Generic;

namespace MyBankApi.Services.Interface
{
    public interface ITransactionService
    {
        Response CreateNewTransaction(Transaction transaction);
        Response FindTransactionByDate(DateTime date);
        Response MakeDeposit(string AccountNumber, decimal Amount, string TransactionPin);
        Response MakeWithdrwal(string AccountNumber, decimal Amount, string TransactionPin);
        Response TransferFund(string FromAccount, string ToAccount, decimal Amount, string TransactionPin);
    }
}
