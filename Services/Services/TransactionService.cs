using IServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonalSpendingAnalysis.Models;
using IRepositories.Interfaces;

namespace Services.Services
{
    public class TransactionService : ITransactionService
    {
        IPersonalSpendingAnalysisRepo repo;

        public TransactionService(IPersonalSpendingAnalysisRepo _repo)
        {
            repo = _repo;
        }


        public List<TransactionModel> GetTransactions()
        {
            throw new NotImplementedException();
        }
    }
}
