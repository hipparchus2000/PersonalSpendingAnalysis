using PersonalSpendingAnalysis.Dtos;
using System.Collections.Generic;
using System;
using Enums;

namespace IRepositories.Interfaces
{
    public interface IPersonalSpendingAnalysisRepo
    {
        void ClearFakeRepo();
        List<TransactionDto> GetTransactions();
        List<CategoryDto> GetCategories();
        List<string> GetCategoryNames();
        TransactionDto GetTransaction(string sha);
        void AddTransaction(TransactionDto dto);
        List<TransactionDto> GetTransactions(global::Enums.orderBy currentOrder);
        List<BudgetDto> GetBudgets();
        void AddNewCategory(CategoryDto categoryDto);
        List<CategoryTotalDto> GetCategoryTotals(DateTime startDate, DateTime endDate, bool showDebitsOnly);
        DateTime GetEarliestTransactionDate();
        List<CategoryTotalDto> GetCategoryTotalsForAllTime();
        double GetNumberOfDaysOfRecordsInSystem();
        ImportResult ImportCategoriesAndTransactions(ExportableDto import);
        void CreateOrUpdateBudgets(List<BudgetDto> p);
        TransactionsWithCategoriesForChartsDto GetTransactionsWithCategoriesForCharts(DateTime start, DateTime end);
        void RemoveCategory(CategoryDto categoryDto);
        void UpdateCategorySearchString(Guid id, string text);
        void UpdateTransactionCategory(Guid id, Guid? categoryId, string subCategory, bool manuallySet = false);
    }
}
