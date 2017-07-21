using PersonalSpendingAnalysis.Models;
using PersonalSpendingAnalysis.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalSpendingAnalysis.Services
{
    class Queries
    {
        internal static List<CategoryTotal> GetCategoryTotals(DateTime startDate, DateTime endDate, bool showDebitsOnly)
        {
            var context = new PersonalSpendingAnalysisRepo();
            var transactions = context.Transaction.Include("Category")
                .Where(x => (x.transactionDate > startDate)
                && (x.transactionDate < endDate)
                );
            var categories = transactions
                .GroupBy(x => new { CategoryName = x.Category.Name })
                .Select(x => new CategoryTotal { 
                    CategoryName = x.Key.CategoryName,
                    Amount = -1 * x.Sum(y => y.amount)
                }).OrderByDescending(x => x.Amount)
                    .ToList();
            if (showDebitsOnly)
                categories = categories.Where(x => x.Amount > 0).ToList();
            return categories;
        }

        internal static List<CategoryTotal> GetCategoryTotalsForAllTime()
        {
            var context = new PersonalSpendingAnalysisRepo();
            var transactions = context.Transaction.Include("Category");
            var categories = transactions
                .GroupBy(x => new { CategoryName = x.Category.Name })
                .Select(x => new CategoryTotal
                {
                    CategoryName = x.Key.CategoryName,
                    Amount = -1 * x.Sum(y => y.amount)
                }).OrderByDescending(x => x.Amount)
                    .ToList();
            return categories;
        }

        internal static double GetNumberOfDaysOfRecordsInSystem()
        {
            var context = new PersonalSpendingAnalysisRepo();
            var earliestDate = context.Transaction.Select(x => x.transactionDate).Min(x => x);
            var latestDate = context.Transaction.Select(x => x.transactionDate).Max(x => x);
            var datespan = latestDate.Subtract(earliestDate);
            return datespan.TotalDays;

        }

        internal static List<BudgetModel> GetBudgets()
        {
            var context = new PersonalSpendingAnalysisRepo();
            var categoryList = context.Budgets.Select(x => new BudgetModel { CategoryName = x.Category.Name, Amount = x.amount } ).ToList();
            return categoryList;
        }

        internal static List<String> GetListOfCategories()
        {
            var context = new PersonalSpendingAnalysisRepo();
            var categoryList = context.Categories.Select(x => x.Name).OrderBy(x=>x).ToList();
            return categoryList;
        }
        
    }
}
