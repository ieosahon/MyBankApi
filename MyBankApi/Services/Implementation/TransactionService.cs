using Microsoft.Extensions.Options;
using MyBankApi.DAL;
using MyBankApi.Models;
using MyBankApi.Services.Interface;
using MyBankApi.Utilities;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace MyBankApi.Services.Implementation
{
    public class TransactionService : ITransactionService
    {
        private readonly BankAppDbContext _context;
        private readonly AppSettings _settings;
        private static string _bankSettlementAccount;
        private readonly IAccountService _accountService;

        public TransactionService(BankAppDbContext context, IOptions<AppSettings> settings, IAccountService accountService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _settings = settings.Value ?? throw new ArgumentNullException(nameof(settings));
            _bankSettlementAccount = _settings.BankSettlementAccount;
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
        }

        public Response CreateNewTransaction(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            _context.SaveChanges();
            var response = new Response()
            {
                ResponseCode = "OK",
                ResponseMessage = "Transaction created successfully",
                Data = null
            };
            return response;
        }

        public Response FindTransactionByDate(DateTime date)
        {
            var transactions = _context.Transactions.Where(t => t.TransactionDate == date);
            var response = new Response()
            {
                ResponseCode = "OK",
                Data = transactions
            };
            return response;
        }

        public Response MakeDeposit(string AccountNumber, decimal Amount, string TransactionPin)
        {
            Account sourceAccount;
            Account destinationAccount;
            Transaction transaction = new();
            Response response = new();

            // check if the user is authenticated

            var user = _accountService.Authenticate(AccountNumber, TransactionPin);
            if (user == null) throw new ApplicationException("Invalid credentials entered");

            // bank settlement gives out the cash
            sourceAccount = _accountService.GetByAccountNumber(_bankSettlementAccount);
            destinationAccount = _accountService.GetByAccountNumber(AccountNumber);

            // to update the source account and destination account

            sourceAccount.AccountBalance -= Amount;
            destinationAccount.AccountBalance += Amount;

            // to check for update in both accounts

            if ((_context.Entry(sourceAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified) && (_context.Entry(destinationAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified))
            {
                // transaction is successful
                transaction.TransactionStatus = TranStatus.Successful;
                response.ResponseMessage = "Transaction is successful";
                response.ResponseCode = "OK";
            }

            else
            {
                transaction.TransactionStatus = TranStatus.Failed;
                response.ResponseCode = "OK";
                response.ResponseMessage = "Transaction failed";
            }

            transaction.TransactionType = TranType.Deposit;
            transaction.DestinationAccount = AccountNumber;
            transaction.SourceAccount = _bankSettlementAccount;
            transaction.TransactionAmount = Amount;
            transaction.TransactionDate = DateTime.Now;
            transaction.TransactionParticular = $"NEW TRANSACTION FROM {JsonConvert.SerializeObject(transaction.SourceAccount)} TO {JsonConvert.SerializeObject(transaction.DestinationAccount)} ON {transaction.TransactionDate}. AMOUNT DEPOSITED {JsonConvert.SerializeObject(transaction.TransactionAmount)}, TRANSACTION TYPE {JsonConvert.SerializeObject(transaction.TransactionType)} TRANSACTION STATUS: {JsonConvert.SerializeObject(transaction.TransactionStatus)}";

            _context.Transactions.Add(transaction);
            _context.SaveChanges();

            return response;
            
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
