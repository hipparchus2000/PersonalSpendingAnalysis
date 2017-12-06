using PersonalSpendingAnalysis.Models;
using System.Collections.Generic;

namespace PersonalSpendingAnalysis.IServices
{
    public interface IBudgetsService
    {
        List<BudgetModel> GetBudgets();
        void CreateOrUpdateBudgets(List<BudgetModel> listOfBudgets);
    }
}