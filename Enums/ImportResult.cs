using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enums
{
    public class ImportResult
    {
        public int numberOfDuplicateCategories { get; set; }
        public int numberOfNewCategories { get; set; }
        public int numberOfNewTransactions { get; set; }
        public int numberOfDuplicateTransactions { get; set; }
    }
}
