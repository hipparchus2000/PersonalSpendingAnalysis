using IRepositories.Interfaces;
using PersonalSpendingAnalysis.Repo.Entities;
using System.Data.Entity;
using System.Linq;
using System.Collections.Generic;
using Enums;
using PersonalSpendingAnalysis.Dtos;
using System;
using Repositories.Contexts;

namespace PersonalSpendingAnalysis.Repo
{
    public class PSAInMemoryContext : DbContext, IPSAContext
    {

        public PSAInMemoryContext() : base("PersonalSpendingAnalysisRepo")
        {
            this.Database.CommandTimeout = 180;
            Accounts = new FakeDbSet<Account>();
            Budgets = new FakeDbSet<Budget>();
            Categories = new FakeDbSet<Category>();
            CategorySearchStrings = new FakeDbSet<CategorySearchString>();
            Imports = new FakeDbSet<Import>();
            Transaction = new FakeDbSet<Transaction>();
        }

        public IDbSet<Account> Accounts { get; set; }
        public IDbSet<Budget> Budgets { get; set; }
        public IDbSet<Category> Categories { get; set; }
        public IDbSet<CategorySearchString> CategorySearchStrings { get; set; }
        public IDbSet<Import> Imports { get; set; }
        public IDbSet<Transaction> Transaction { get; set; }

    }
}
