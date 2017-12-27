using System.Collections.Generic;
using System.Linq;
using PersonalSpendingAnalysis.Models;
using PersonalSpendingAnalysis.IServices;
using IRepositories.Interfaces;
using PersonalSpendingAnalysis.Dtos;

namespace PersonalSpendingAnalysis.Services
{
    public class BudgetsService : IBudgetsService
    {
        IPersonalSpendingAnalysisRepo repo;

        public BudgetsService(IPersonalSpendingAnalysisRepo _repo)
        {
            repo = _repo;
        }

        public List<BudgetModel> GetBudgets()
        {
            return repo.GetBudgets().Select(x=>new BudgetModel {
                Amount = x.Amount, CategoryName = x.CategoryName
            }).ToList();
        }

        public void CreateOrUpdateBudgets(List<BudgetModel> listOfBudgets)
        {
            repo.CreateOrUpdateBudgets(listOfBudgets.Select(x=>new BudgetDto
            {
                Amount = x.Amount,
                CategoryName = x.CategoryName
            }).ToList());
            
        }
    }
}
