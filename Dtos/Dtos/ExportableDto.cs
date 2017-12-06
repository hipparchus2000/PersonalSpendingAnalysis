using System.Collections.Generic;

namespace PersonalSpendingAnalysis.Dtos
{
    public class ExportableDto
    {
        public IEnumerable<CategoryDto> categories;
        public IEnumerable<TransactionDto> transactions;
    }
}