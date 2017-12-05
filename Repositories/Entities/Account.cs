using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalSpendingAnalysis.Repo.Entities
{
    public class Account
    {
        public Account()
        {
            Id = Guid.NewGuid();
        }
        
        public Guid Id { get; set; }
        public string BankName { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }

        public List<Transaction> Transactions {get;set;}
        public List<Import> Imports { get; set; }

    }
}
