using System.Collections.Generic;

namespace PersonalSpendingAnalysis.Models
{
    public class ExportableModel
    {
        public List<TransactionModel> transactions;
        public List<CategoryModel> categories;
    }
}
