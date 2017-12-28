using IRepositories.Interfaces;
using PersonalSpendingAnalysis.Repo.Entities;
using System.Data.Entity;
using System.Linq;
using System.Collections.Generic;
using Enums;
using PersonalSpendingAnalysis.Dtos;
using System;

namespace PersonalSpendingAnalysis.Repo
{
    public class PSAContext : DbContext, IPSAContext
    {

        public PSAContext() : base("PersonalSpendingAnalysisRepo")
        {
            this.Database.CommandTimeout = 180;
        }

        public IDbSet<Account> Accounts { get; set; }
        public IDbSet<Budget> Budgets { get; set; }
        public IDbSet<Category> Categories { get; set; }
        public IDbSet<CategorySearchString> CategorySearchStrings { get; set; }
        public IDbSet<Import> Imports { get; set; }
        public IDbSet<Transaction> Transaction { get; set; }

    }
}
