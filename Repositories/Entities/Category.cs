using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalSpendingAnalysis.Repo.Entities
{
    public class Category
    {
        public Category()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string SearchString { get; set; }
        
    }
}
