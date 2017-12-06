using IServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonalSpendingAnalysis.Models;
using IRepositories.Interfaces;
using Enums;
using Models.Models;

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
            var transactions = repo.GetTransactions().Select(x=>new TransactionModel {
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
            }).ToList();
            return transactions;
        }

        public List<TransactionModel> GetTransactions(orderBy currentOrder)
        {
            var transactions = repo.GetTransactions(currentOrder).Select(x => new TransactionModel
            {
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
            }).ToList();
            return transactions;
        }
    }
}
