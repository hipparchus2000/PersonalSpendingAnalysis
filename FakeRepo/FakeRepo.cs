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
        List<CategoryDto> fakeCategories;
        List<TransactionDto> fakeTransactions;
        List<BudgetDto> fakeBudgets;

        public FakeRepo()
        {
            fakeCategories = new List<CategoryDto>();
            fakeTransactions = new List<TransactionDto>();
            fakeBudgets = new List<BudgetDto>();
        }

        public void AddNewCategory(CategoryDto categoryDto)
        {
            fakeCategories.Add(categoryDto);
        }

        public void AddTransaction(TransactionDto dto)
        {
            fakeTransactions.Add(dto);
        }

        public void CreateOrUpdateBudgets(List<BudgetDto> p)
        {
            foreach(var budget in p)
            {
                var testBudget = fakeBudgets.SingleOrDefault(x => x.CategoryName == budget.CategoryName);
                if (testBudget == null)
                {
                    fakeBudgets.Add(budget);
                }
                else
                {
                    fakeBudgets.Remove(testBudget);
                    fakeBudgets.Add(budget);
                }
            }
        }

        public List<BudgetDto> GetBudgets()
        {
            return fakeBudgets.OrderBy(x => x.CategoryName).ToList();
        }

        public List<CategoryDto> GetCategories()
        {
            return fakeCategories.OrderBy(x => x.Name).ToList();
        }

        public List<string> GetCategoryNames()
        {
            return fakeCategories.Select(x => x.Name).ToList();
        }

        public List<CategoryTotalDto> GetCategoryTotals(DateTime startDate, DateTime endDate, bool showDebitsOnly)
        {
            var transactions = fakeTransactions
                .Where(x => (x.transactionDate > startDate)
                && (x.transactionDate < endDate)
                );

            var categories = transactions
                .GroupBy(x => new { CategoryName = x.Category.Name })
                .Select(x => new CategoryTotalDto
                {
                    CategoryName = x.Key.CategoryName,
                    Amount = -1 * x.Sum(y => y.amount)
                }).OrderByDescending(x => x.Amount)
                    .ToList();
            if (showDebitsOnly)
                categories = categories.Where(x => x.Amount > 0).ToList();
            return categories;
        }

        public List<CategoryTotalDto> GetCategoryTotalsForAllTime()
        {
            var transactions = fakeTransactions;
            var categories = transactions
                .GroupBy(x => new { CategoryName = x.Category.Name })
                .Select(x => new CategoryTotalDto
                {
                    CategoryName = x.Key.CategoryName,
                    Amount = -1 * x.Sum(y => y.amount)
                }).OrderByDescending(x => x.Amount)
                    .ToList();
            return categories;
        }

        public DateTime GetEarliestTransactionDate()
        {
            return fakeTransactions.Min(x => x.transactionDate);
        }

        public double GetNumberOfDaysOfRecordsInSystem()
        {
            var earliestDate = fakeTransactions.Select(x => x.transactionDate).Min(x => x);
            var latestDate = fakeTransactions.Select(x => x.transactionDate).Max(x => x);
            var datespan = latestDate.Subtract(earliestDate);
            return datespan.TotalDays;
        }

        public TransactionDto GetTransaction(string id)
        {
            Guid ID = Guid.Parse(id);
            return fakeTransactions.Single(x => x.Id == ID);
        }

        public List<TransactionDto> GetTransactions()
        {
            return fakeTransactions;
        }

        public List<TransactionDto> GetTransactions(orderBy currentOrder)
        {
            var datarows = new List<TransactionDto>().ToList();

            switch (currentOrder)
            {
                case orderBy.transactionDateDescending:
                    datarows = fakeTransactions.OrderByDescending(x => x.transactionDate).ToList();
                    break;
                case orderBy.transactionDateAscending:
                    datarows = fakeTransactions.OrderBy(x => x.transactionDate).ToList();
                    break;
                case orderBy.amountAscending:
                    datarows = fakeTransactions.OrderBy(x => x.amount).ToList();
                    break;
                case orderBy.amountDescending:
                    datarows = fakeTransactions.OrderByDescending(x => x.amount).ToList();
                    break;
                case orderBy.categoryAscending:
                    datarows = fakeTransactions.OrderBy(x => x.Category == null ? "" : x.Category.Name).ToList();
                    break;
                case orderBy.categoryDescending:
                    datarows = fakeTransactions.OrderByDescending(x => x.Category == null ? "" : x.Category.Name).ToList();
                    break;
            }
            return datarows;
        }

        public TransactionsWithCategoriesForChartsDto GetTransactionsWithCategoriesForCharts(DateTime start, DateTime end)
        {
            var transactions = fakeTransactions
                .Where(x => (x.transactionDate > start)
                    && (x.transactionDate < end)
                );
            var categories = transactions
                .GroupBy(x => new { CategoryName = x.Category.Name })
                .Select(x => new
                {
                    CategoryName = x.Key.CategoryName,
                    Amount = x.Sum(y => y.amount)
                }).OrderByDescending(x => x.Amount)
                    .ToList();
            var result = new TransactionsWithCategoriesForChartsDto();
            result.Transactions = fakeTransactions.Select(x => new TransactionDto
            {
                AccountId = x.AccountId,
                amount = x.amount,
                Category = x.Category == null ? null : new CategoryDto
                {
                    Id = x.Category.Id,
                    Name = x.Category.Name,
                    SearchString = x.Category.SearchString
                },
                ManualCategory = x.ManualCategory,
                Id = x.Id,
                CategoryId = x.CategoryId,
                Notes = x.Notes,
                SHA256 = x.SHA256,
                SubCategory = x.SubCategory,
                transactionDate = x.transactionDate
            }).ToList();
            result.Categories = fakeCategories.Select(x => new CategoryDto
            {
                Id = x.Id,
                Name = x.Name,
                SearchString = x.SearchString
            }).ToList();
            return result;
        }

        public ImportResult ImportCategoriesAndTransactions(ExportableDto import)
        {
            var result = new ImportResult();

            //do the categories first to avoid foreign key issues
            foreach (var category in import.categories)
            {
                if (fakeCategories.SingleOrDefault(x => x.Id == category.Id) != null)
                {
                    //this method aggregates search strings - todo perhaps give user the choice to aggregate or
                    //replace or keep search strings.
                    result.numberOfDuplicateCategories++;
                    var oldCategory = fakeCategories.Single(x => x.Id == category.Id);
                    var searchStrings = oldCategory.SearchString.Split(',').ToList();
                    searchStrings.AddRange(category.SearchString.Split(','));
                    var uniqueSearchStrings = searchStrings.Distinct();
                    oldCategory.SearchString = string.Join(",", uniqueSearchStrings);
                }
                else
                {
                    fakeCategories.Add(new CategoryDto
                    {
                        Id = category.Id,
                        Name = category.Name,
                        SearchString = category.Name
                    });
                    result.numberOfNewCategories++;
                }
            }

            foreach (var transaction in import.transactions)
            {
                if (fakeTransactions.SingleOrDefault(x => x.SHA256 == transaction.SHA256) == null)
                {
                    //import the transaction
                    fakeTransactions.Add(new TransactionDto
                    {
                        AccountId = transaction.AccountId,
                        amount = transaction.amount,
                        Category = null, //avoid making duplicate category
                        ManualCategory = transaction.ManualCategory,
                        Id = transaction.Id,
                        CategoryId = transaction.CategoryId,
                        Notes = transaction.Notes,
                        SHA256 = transaction.SHA256,
                        SubCategory = transaction.SubCategory,
                        transactionDate = transaction.transactionDate
                    });
                    result.numberOfNewTransactions++;
                }
                else
                {
                    //transaction is already here
                    result.numberOfDuplicateTransactions++;
                }
            }

            return result;
        }

        public void RemoveCategory(CategoryDto categoryDto)
        {
            var dto = fakeCategories.Single(x => x.Id == categoryDto.Id);
            fakeCategories.Remove(dto);
        }

        public void UpdateCategorySearchString(Guid id, string text)
        {
            var category = fakeCategories.Single(x => x.Id == id);
            var newCategory = new CategoryDto
            {
                Id = id,
                Name = category.Name,
                SearchString = category.SearchString + "," + text
            };
            fakeCategories.Remove(category);
            fakeCategories.Add(newCategory);
        }

        public void UpdateTransactionCategory(Guid id, Guid? categoryId, string subCategory, bool manuallySet = false)
        {
            var transaction = fakeTransactions.Single(x => x.Id == id);
            var category = categoryId == null? null : fakeCategories.SingleOrDefault(x => x.Id == categoryId);

            var replacementTransaction = new TransactionDto
            {
                CategoryId = categoryId, AccountId = transaction.AccountId, amount = transaction.amount,
                Id = id, ManualCategory = manuallySet, SHA256 = transaction.SHA256, SubCategory = subCategory,
                Notes = transaction.Notes, transactionDate = transaction.transactionDate,
                Category = category 
            };
            fakeTransactions.Remove(transaction);
            fakeTransactions.Add(replacementTransaction);
        }
    }
}
