using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalSpendingAnalysis.Repo.Entities
{
    public class CategorySearchString
    {
        public CategorySearchString()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public string String { get; set; }


    }
}
