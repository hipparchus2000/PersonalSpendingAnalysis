using System.Collections.Generic;

namespace PersonalSpendingAnalysis.Dtos
{
    public class TransactionsWithCategoriesForChartsDto
    {
        public List<CategoryDto> Categories { get; set; }
        public List<TransactionDto> Transactions { get; set; }
        public List<CategoryTotalDto> CategoryTotals { get; set; }
    }
}