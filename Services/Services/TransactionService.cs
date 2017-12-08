using IServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonalSpendingAnalysis.Models;
using IRepositories.Interfaces;
using Enums;
using PersonalSpendingAnalysis.Dtos;

namespace Services.Services
{
    public class TransactionService : ITransactionService
    {
        IPersonalSpendingAnalysisRepo repo;

        public TransactionService(IPersonalSpendingAnalysisRepo _repo)
        {
            repo = _repo;
        }

        public void AddNewTransaction(TransactionModel model)
        {
            var dto = new TransactionDto
            {
                AccountId = model.AccountId,
                amount = model.amount,
                CategoryId = model.CategoryId,
                Id = model.Id,
                ManualCategory = model.ManualCategory,
                Notes = model.Notes,
                SHA256 = model.SHA256,
                SubCategory = model.SubCategory,
                transactionDate = model.transactionDate
            };
            repo.AddTransaction(dto);
        }

        public DateTime GetEarliestTransactionDate()
        {
            return repo.GetEarliestTransactionDate();
        }

        public TransactionModel GetTransaction(Guid transactionId)
        {
            var dto = repo.GetTransaction(transactionId.ToString());
            return new TransactionModel
            {
                CategoryId = dto.CategoryId,
                AccountId = dto.AccountId,
                amount = dto.amount,
                Id = dto.Id,
                ManualCategory = dto.ManualCategory,
                Notes = dto.Notes,
                SHA256 = dto.SHA256,
                SubCategory = dto.SubCategory,
                transactionDate = dto.transactionDate,
                Category = dto.Category == null ? null : new CategoryModel
                {
                    Id = dto.Category.Id, Name = dto.Category.Name, SearchString = dto.Category.Name
                }
            };
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

        public void UpdateTransactionCategory(Guid id, Guid? categoryId, string subCategory, bool manuallySet = false)
        {
            repo.UpdateTransactionCategory(id, categoryId, subCategory, manuallySet);
        }


    }
}
