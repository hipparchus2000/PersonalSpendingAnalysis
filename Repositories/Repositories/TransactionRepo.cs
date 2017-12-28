using IRepositories.Interfaces;
using PersonalSpendingAnalysis.Repo.Entities;
using System.Data.Entity;
using System.Linq;
using System.Collections.Generic;
using Enums;
using PersonalSpendingAnalysis.Dtos;
using System;

namespace PersonalSpendingAnalysis.Repo
{
    public class TransactionRepo : ITransactionRepo
    {
        private PSAContext context;

        public TransactionRepo(PSAContext _context) 
        {
            context = _context;
        }

        public void AddTransaction(TransactionDto dto)
        {
            context.Transaction.Add(new Transaction
            {
                AccountId = null,
                amount = dto.amount,
                CategoryId = dto.CategoryId,
                ManualCategory = dto.ManualCategory,
                SHA256 = dto.SHA256,
                SubCategory = dto.SubCategory,
                Id = dto.Id,
                Notes = dto.Notes,
                transactionDate = dto.transactionDate
            });
            context.SaveChanges();
        }

        public double GetNumberOfDaysOfRecordsInSystem()
        {
            var earliestDate = context.Transaction.Select(x => x.transactionDate).Min(x => x);
            var latestDate = context.Transaction.Select(x => x.transactionDate).Max(x => x);
            var datespan = latestDate.Subtract(earliestDate);
            return datespan.TotalDays;
        }

        /// <summary>
        /// Fetches a transaction or Null given the SHA256 for the record
        /// </summary>
        /// <param name="sha"></param>
        /// <returns>the matching TransactionDto or null</returns>
        public TransactionDto GetTransaction(string sha)
        {
            var existingRowForThisSHA256 = context.Transaction.SingleOrDefault(x => x.SHA256 == sha);
            if (existingRowForThisSHA256 == null) return null;
            return MapTransactionToTransactionDto(existingRowForThisSHA256);
        }

        /// <summary>
        /// Fetches a transaction or Null given the SHA256 for the record
        /// </summary>
        /// <param name="id"></param>
        /// <returns>the matching TransactionDto or null</returns>
        public TransactionDto GetTransaction(Guid id)
        {
            var entity = context.Transaction.SingleOrDefault(x => x.Id == id);
            if (entity == null) return null;
            return MapTransactionToTransactionDto(entity);
        }

        private TransactionDto MapTransactionToTransactionDto(Transaction x)
        {
            return new TransactionDto
            {
                AccountId = x.AccountId,
                amount = x.amount,
                Category = x.Category == null ? null: new CategoryDto {
                    Id = x.Category.Id, Name = x.Category.Name,
                    SearchString = x.Category.SearchString
                },
                ManualCategory = x.ManualCategory,
                Id = x.Id,
                CategoryId = x.CategoryId,
                Notes = x.Notes,
                SHA256 = x.SHA256,
                SubCategory = x.SubCategory,
                transactionDate =x.transactionDate
            };
        }

        public List<TransactionDto> GetTransactions()
        {
            return context.Transaction.Select(x=> new TransactionDto
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
        }

        public List<TransactionDto> GetTransactions(orderBy currentOrder)
        {
            var datarows = new List<Transaction>().ToList();

            switch (currentOrder)
            {
                case orderBy.transactionDateDescending:
                    datarows = context.Transaction.Include("Category").OrderByDescending(x => x.transactionDate).ToList();
                    break;
                case orderBy.transactionDateAscending:
                    datarows = context.Transaction.Include("Category").OrderBy(x => x.transactionDate).ToList();
                    break;
                case orderBy.amountAscending:
                    datarows = context.Transaction.Include("Category").OrderBy(x => x.amount).ToList();
                    break;
                case orderBy.amountDescending:
                    datarows = context.Transaction.Include("Category").OrderByDescending(x => x.amount).ToList();
                    break;
                case orderBy.categoryAscending:
                    datarows = context.Transaction.Include("Category").OrderBy(x => x.Category == null ? "" : x.Category.Name).ToList();
                    break;
                case orderBy.categoryDescending:
                    datarows = context.Transaction.Include("Category").OrderByDescending(x => x.Category == null ? "" : x.Category.Name).ToList();
                    break;
            }

            return datarows.Select(x=> new TransactionDto
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

        }

        public ImportResult ImportCategoriesAndTransactions(ExportableDto import)
        {
            var result = new ImportResult();

            //do the categories first to avoid foreign key issues
            foreach (var category in import.categories)
            {
                if (context.Categories.SingleOrDefault(x => x.Id == category.Id) != null)
                {
                    //this method aggregates search strings - todo perhaps give user the choice to aggregate or
                    //replace or keep search strings.
                    result.numberOfDuplicateCategories++;
                    var oldCategory = context.Categories.Single(x => x.Id == category.Id);
                    var searchStrings = oldCategory.SearchString.Split(',').ToList();
                    searchStrings.AddRange(category.SearchString.Split(','));
                    var uniqueSearchStrings = searchStrings.Distinct();
                    oldCategory.SearchString = string.Join(",", uniqueSearchStrings);
                }
                else
                {
                    context.Categories.Add(new Category {
                        Id = category.Id,
                        Name = category.Name,
                        SearchString = category.Name
                    });
                    context.SaveChanges();
                    result.numberOfNewCategories++;
                }
            }

            foreach (var transaction in import.transactions)
            {
                if (context.Transaction.SingleOrDefault(x => x.SHA256 == transaction.SHA256) == null)
                {
                    //import the transaction
                    context.Transaction.Add(new Transaction {
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
                    context.SaveChanges();
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


        public TransactionsWithCategoriesForChartsDto GetTransactionsWithCategoriesForCharts(DateTime start, DateTime end)
        {
            var transactions = context.Transaction.Include("Category")
                .Where(x => (x.transactionDate > start )    
                && (x.transactionDate < end )
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
            result.Transactions = context.Transaction.Include(x=>x.Category).Select(x=> new TransactionDto
            {
                AccountId = x.AccountId,
                amount = x.amount,
                Category = x.Category == null? null : new CategoryDto {
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
            result.Categories = context.Categories.Select(x=>new CategoryDto
            {
                Id = x.Id,
                Name = x.Name,
                SearchString = x.SearchString
            }).ToList();
            result.CategoryTotals = categories.Select(x => new CategoryTotalDto
            {
                CategoryName = x.CategoryName,
                Amount = x.Amount
            }).ToList();

            return result;
        }

        public DateTime GetEarliestTransactionDate()
        {
            return context.Transaction.Min(x => x.transactionDate);
        }
        
        public void UpdateTransactionCategory(Guid id, Guid? categoryId, string subCategory, bool manuallySet = false)
        {
            var transaction = context.Transaction.Single(x => x.Id == id);
            transaction.CategoryId = categoryId;
            transaction.SubCategory = subCategory;
            transaction.ManualCategory = manuallySet;
            context.SaveChanges();
        }


    }
}
