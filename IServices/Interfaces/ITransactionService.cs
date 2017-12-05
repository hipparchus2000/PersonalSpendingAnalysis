using PersonalSpendingAnalysis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServices.Interfaces
{
    public interface ITransactionService
    {
        List<TransactionModel> GetTransactions();
    }
}
