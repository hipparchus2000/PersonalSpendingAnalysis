using System.Data.Entity;
using PersonalSpendingAnalysis.Repo.Entities;

namespace PersonalSpendingAnalysis.Repo
{
    public interface IPSAContext
    {
        IDbSet<Account> Accounts { get; set; }
        IDbSet<Budget> Budgets { get; set; }
        IDbSet<Category> Categories { get; set; }
        IDbSet<CategorySearchString> CategorySearchStrings { get; set; }
        IDbSet<Import> Imports { get; set; }
        IDbSet<Transaction> Transaction { get; set; }
    }
}