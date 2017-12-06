using System.Collections.Generic;

namespace PersonalSpendingAnalysis.Models
{
    public class TransactionsAndCategoriesForChartsModel
    {
        public List<CategoryModel> Categories { get; set; }
        public List<TransactionModel> Transactions { get; set; }
        public List<CategoryTotal> CategoryTotals { get; set; }
    }
}