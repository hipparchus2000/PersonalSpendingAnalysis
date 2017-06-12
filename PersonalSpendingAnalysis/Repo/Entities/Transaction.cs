using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalSpendingAnalysis.Repo.Entities
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public decimal amount { get; set; }
        public DateTime transactionDate { get; set; }
        public string Notes { get; set; }
        public Category Category { get; set; }

    }
}
