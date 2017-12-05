using PersonalSpendingAnalysis.Models;
using System;
using System.Collections.Generic;

namespace PersonalSpendingAnalysis.IServices
{
    public interface IQueryService
    {
        List<CategoryTotal> GetCategoryTotals(DateTime startDate, DateTime endDate, bool showDebitsOnly);
        List<CategoryTotal> GetCategoryTotalsForAllTime();
        double GetNumberOfDaysOfRecordsInSystem();
        List<BudgetModel> GetBudgets();
        List<String> GetListOfCategories();
    }
}
