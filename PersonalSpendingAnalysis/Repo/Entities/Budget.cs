using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalSpendingAnalysis.Repo.Entities
{
    public class Budget
    {
        public Budget()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public decimal amount { get; set; }
        public Guid? CategoryId { get; set; }
        public Category Category { get; set; }
        //public String SubCategory { get; set; } //subcategory is search string found
        public Guid? AccountId { get; set; }
    }
}
