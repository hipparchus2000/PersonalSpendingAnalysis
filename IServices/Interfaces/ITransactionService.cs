using PersonalSpendingAnalysis.Models;
using System;
using System.Collections.Generic;

namespace IServices.Interfaces
{
    public interface ITransactionService
    {
        List<TransactionModel> GetTransactions();
        List<TransactionModel> GetTransactions(global::Enums.orderBy currentOrder);
        DateTime GetEarliestTransactionDate();
        void UpdateTransactionCategory(Guid id, Guid? categoryId, string subCategory, bool manuallySet = false);
        void AddNewTransaction(TransactionModel remoteTransaction);
        TransactionModel GetTransaction(Guid transactionId);
    }
}
