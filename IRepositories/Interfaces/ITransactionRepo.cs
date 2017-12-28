using System;
using System.Collections.Generic;
using Enums;
using PersonalSpendingAnalysis.Dtos;

namespace PersonalSpendingAnalysis.Repo
{
    public interface ITransactionRepo
    {
        void AddTransaction(TransactionDto dto);
        DateTime GetEarliestTransactionDate();
        double GetNumberOfDaysOfRecordsInSystem();
        TransactionDto GetTransaction(Guid id);
        TransactionDto GetTransaction(string sha);
        List<TransactionDto> GetTransactions();
        List<TransactionDto> GetTransactions(orderBy currentOrder);
        TransactionsWithCategoriesForChartsDto GetTransactionsWithCategoriesForCharts(DateTime start, DateTime end);
        ImportResult ImportCategoriesAndTransactions(ExportableDto import);
        void UpdateTransactionCategory(Guid id, Guid? categoryId, string subCategory, bool manuallySet = false);
    }
}