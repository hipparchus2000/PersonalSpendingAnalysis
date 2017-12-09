using IRepositories.Interfaces;
using PersonalSpendingAnalysis.IServices;
using PersonalSpendingAnalysis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using PersonalSpendingAnalysis.Dtos;

namespace PersonalSpendingAnalysis.Services
{
    
    public class QueryService : IQueryService
    {
        IPersonalSpendingAnalysisRepo repo;

        public QueryService(IPersonalSpendingAnalysisRepo _repo)
        {
            repo = _repo;
        }

        public List<CategoryTotal> GetCategoryTotals(DateTime startDate, DateTime endDate, bool showDebitsOnly)
        {
            var dtos = repo.GetCategoryTotals(startDate, endDate, showDebitsOnly);
            return MapCategoryTotalDtoToCategoryTotal(dtos);
        }

        private List<CategoryTotal> MapCategoryTotalDtoToCategoryTotal(List<CategoryTotalDto> dtos)
        {
            return dtos.Select(x => new CategoryTotal
            {
                Amount = x.Amount,
                CategoryName = x.CategoryName
            }).ToList();
        }

        public List<CategoryTotal> GetCategoryTotalsForAllTime()
        {
            var list = repo.GetCategoryTotalsForAllTime();
            return MapCategoryTotalDtoToCategoryTotal(list);
        }

        public double GetNumberOfDaysOfRecordsInSystem()
        {
            var result = repo.GetNumberOfDaysOfRecordsInSystem();
            return result;
        }

        public TransactionsAndCategoriesForChartsModel GetTransactionsWithCategoriesForCharts(DateTime start, DateTime end)
        {
            var dto = repo.GetTransactionsWithCategoriesForCharts(start, end);
            var model = new TransactionsAndCategoriesForChartsModel
            {
                Categories = dto.Categories.Select(x=> new CategoryModel {
                    Id = x.Id,
                    Name = x.Name,
                    SearchString = x.SearchString
                }).ToList(),
                Transactions = dto.Transactions.Select(x=>new TransactionModel {
                    AccountId = x.AccountId,
                    amount = x.amount,
                    Category = x.Category == null ? null : new CategoryModel
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
                }).ToList(),
                CategoryTotals = dto.CategoryTotals.Select(x=>new CategoryTotal
                {
                    CategoryName = x.CategoryName, Amount = x.Amount
                }).ToList()
            };
            return model;
        }
    }
}
