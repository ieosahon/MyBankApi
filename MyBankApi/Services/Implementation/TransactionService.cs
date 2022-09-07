using MyBankApi.DAL;
using MyBankApi.Models;
using MyBankApi.Services.Interface;
using System;

namespace MyBankApi.Services.Implementation
{
    public class TransactionService : ITransactionService
    {
        private readonly BankAppDbContext _context;

        public TransactionService(BankAppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Response CreateNewTransaction(Transaction transaction)
        {
            throw new NotImplementedException();
        }

        public Response FindTransactionByDate(DateTime date)
        {
            throw new NotImplementedException();
        }

        public Response MakeDeposit(string AccountNumber, decimal Amount, string TransactionPin)
        {
            throw new NotImplementedException();
        }

        public Response MakeWithdrwal(string AccountNumber, decimal Amount, string TransactionPin)
        {
            throw new NotImplementedException();
        }

        public Response TransferFund(string FromAccount, string ToAccount, decimal Amount, string TransactionPin)
        {
            throw new NotImplementedException();
        }
    }
}
