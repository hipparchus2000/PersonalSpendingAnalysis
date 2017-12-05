using IRepositories.Interfaces;
using PersonalSpendingAnalysis.Repo.Entities;
using System.Data.Entity;
using System.Linq;
using PersonalSpendingAnalysis.Services;
using System.Collections.Generic;

namespace PersonalSpendingAnalysis.Repo
{
    public class PersonalSpendingAnalysisRepo : DbContext , IPersonalSpendingAnalysisRepo
    {

        public PersonalSpendingAnalysisRepo() : base("PersonalSpendingAnalysisRepo")
        {
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategorySearchString> CategorySearchStrings { get; set; }
        public DbSet<Import> Imports { get; set; }
        public DbSet<Transaction> Transaction { get; set; }

        public void AddTransaction(TransactionDto dto)
        {
            Transaction.Add(new Repo.Entities.Transaction
            {
                //todo use automapper instead
                AccountId = null,
                amount = dto.amount,
                CategoryId = dto.CategoryId,
                ManualCategory = dto.ManualCategory,

            });
            SaveChanges();
        }

        public System.Collections.Generic.List<CategoryDto> GetCategories()
        {
            //todo project using automapper
            return Categories.ToList();
        }

        public TransactionDto GetTransaction(string id)
        {
            //todo use automapper
            var existingRowForThisSHA256 = Transaction.SingleOrDefault(x => x.SHA256 == id);
            return existingRowForThisSHA256;
        }

        public List<TransactionDto> GetTransactions()
        {
            //todo project using automapper 
            return Transaction.ToList();
        }


    }
}
