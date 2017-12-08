using IRepositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Enums;
using PersonalSpendingAnalysis.Dtos;

namespace Repositories
{
    public class FakeRepo : IPersonalSpendingAnalysisRepo
    {
        List<CategoryDto> categories;
        List<TransactionDto> transactions;
        List<BudgetDto> budgets;

        public void AddNewCategory(CategoryDto categoryDto)
        {
            categories.Add(categoryDto);
        }

        public void AddTransaction(TransactionDto dto)
        {
            transactions.Add(dto);
        }

        public void CreateOrUpdateBudgets(List<BudgetDto> p)
        {
            foreach(var budget in p)
            {
                var testBudget = budgets.SingleOrDefault(x => x.CategoryName == budget.CategoryName);
                if (testBudget == null)
                {
                    budgets.Add(budget);
                }
                else
                {
                    budgets.Remove(testBudget);
                    budgets.Add(budget);
                }
            }
        }

        public List<BudgetDto> GetBudgets()
        {
            return budgets;
        }

        public List<CategoryDto> GetCategories()
        {
            return categories;
        }

        public List<string> GetCategoryNames()
        {
            return categories.Select(x => x.Name).ToList();
        }

        public List<CategoryTotalDto> GetCategoryTotals(DateTime startDate, DateTime endDate, bool showDebitsOnly)
        {
            throw new NotImplementedException();
        }

        public List<CategoryTotalDto> GetCategoryTotalsForAllTime()
        {
            throw new NotImplementedException();
        }

        public DateTime GetEarliestTransactionDate()
        {
            throw new NotImplementedException();
        }

        public double GetNumberOfDaysOfRecordsInSystem()
        {
            throw new NotImplementedException();
        }

        public TransactionDto GetTransaction(string id)
        {
            throw new NotImplementedException();
        }

        public List<TransactionDto> GetTransactions()
        {
            return transactions;
        }

        public List<TransactionDto> GetTransactions(orderBy currentOrder)
        {
            throw new NotImplementedException();
        }

        public TransactionsWithCategoriesForChartsDto GetTransactionsWithCategoriesForCharts(DateTime start, DateTime end)
        {
            throw new NotImplementedException();
        }

        public ImportResult ImportCategoriesAndTransactions(ExportableDto import)
        {
            throw new NotImplementedException();
        }

        public void RemoveCategory(CategoryDto categoryDto)
        {
            categories.Remove(categoryDto);
        }

        public void UpdateCategorySearchString(Guid id, string text)
        {
            var category = categories.Single(x => x.Id == id);
            var newCategory = new CategoryDto
            {
                Id = id,
                Name = category.Name,
                SearchString = category.SearchString + "," + text
            };
            categories.Remove(category);
        }

        public void UpdateTransactionCategory(Guid id, Guid? categoryId, string subCategory, bool manuallySet = false)
        {
            throw new NotImplementedException();
        }
    }
}
