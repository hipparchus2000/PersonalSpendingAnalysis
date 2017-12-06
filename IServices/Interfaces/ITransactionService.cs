using PersonalSpendingAnalysis.Models;
using System;
using System.Collections.Generic;

namespace IServices.Interfaces
{
    public interface ITransactionService
    {
        List<TransactionModel> GetTransactions();
        List<TransactionModel> GetTransactions(global::Enums.orderBy currentOrder);
    }
}
