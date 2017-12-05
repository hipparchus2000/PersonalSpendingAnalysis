using PersonalSpendingAnalysis.Models;
using System.Collections.Generic;

namespace PersonalSpendingAnalysis.IServices
{
    public interface IAggregatesService
    {
        void CreateOrUpdateBudgets(List<BudgetModel> listOfBudgets);
    }
}