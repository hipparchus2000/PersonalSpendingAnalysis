using PersonalSpendingAnalysis.Repo.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalSpendingAnalysis.Repo
{
    public class PersonalSpendingAnalysisRepo : DbContext
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

    }
}
