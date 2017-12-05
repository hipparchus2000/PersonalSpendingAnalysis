using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalSpendingAnalysis.Repo.Entities
{
    public class Import
    {
        public Import()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public string Filename { get; set; }
        public DateTime importDate { get; set; }
        public string SHA256 { get; set; }
        
    }
}
