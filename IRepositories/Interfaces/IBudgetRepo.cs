using PersonalSpendingAnalysis.Dtos;
using System.Collections.Generic;

namespace PersonalSpendingAnalysis.Repo
{
    public interface IBudgetRepo
    {
        List<BudgetDto> GetBudgets();
        void CreateOrUpdateBudgets(List<BudgetDto> listOfBudgets);
    }
}