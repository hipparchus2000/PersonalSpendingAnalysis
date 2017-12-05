using IRepositories.Interfaces;
using PersonalSpendingAnalysis.IServices;
using PersonalSpendingAnalysis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity;

namespace PersonalSpendingAnalysis.Services
{
    
    //todo rename this to QueryServiceService
    public class QueryService : IQueryService
    {
        IPersonalSpendingAnalysisRepo repo;

        public QueryService(IPersonalSpendingAnalysisRepo _repo)
        {
            repo = _repo;
        }

        public List<CategoryTotal> GetCategoryTotals(DateTime startDate, DateTime endDate, bool showDebitsOnly)
        {
            //todo move this to repo
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

        public List<CategoryTotal> GetCategoryTotalsForAllTime()
        {
            //todo move this to repo.
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

        public double GetNumberOfDaysOfRecordsInSystem()
        {
            //todo move this to repo
            var earliestDate = context.Transaction.Select(x => x.transactionDate).Min(x => x);
            var latestDate = context.Transaction.Select(x => x.transactionDate).Max(x => x);
            var datespan = latestDate.Subtract(earliestDate);
            return datespan.TotalDays;

        }

        public List<BudgetModel> GetBudgets()
        {
            //todo move this to repo
            var categoryList = context.Budgets.Select(x => new BudgetModel { CategoryName = x.Category.Name, Amount = x.amount } ).ToList();
            return categoryList;
        }

        public List<String> GetListOfCategories()
        {
            //todo move this to repo
            var categoryList = context.Categories.Select(x => x.Name).OrderBy(x=>x).ToList();
            return categoryList;
        }
        
    }
}
