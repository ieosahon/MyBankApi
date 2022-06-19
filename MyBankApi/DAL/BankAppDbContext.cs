using Microsoft.EntityFrameworkCore;
using MyBankApi.Models;

namespace MyBankApi.DAL
{
    public class BankAppDbContext : DbContext
    {
        public BankAppDbContext(DbContextOptions<BankAppDbContext> dbContext): base(dbContext)
        {
            
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
    }
}
