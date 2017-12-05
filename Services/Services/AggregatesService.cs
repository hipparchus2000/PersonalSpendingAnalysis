using System.Collections.Generic;
using System.Linq;
using PersonalSpendingAnalysis.Models;
using PersonalSpendingAnalysis.IServices;
using Unity;
using IRepositories.Interfaces;

namespace PersonalSpendingAnalysis.Services
{
    //todo rename this to aggregates service
    public class AggregatesService : IAggregatesService
    {
        IPersonalSpendingAnalysisRepo repo;

        public AggregatesService(IPersonalSpendingAnalysisRepo _repo)
        {
            repo = _repo;
        }

        public void CreateOrUpdateBudgets(List<BudgetModel> listOfBudgets)
        {
            //todo move this to repo
            foreach(var budget in listOfBudgets)
            {
                var existingBudget = context.Budgets.Include("Category").FirstOrDefault(x => x.Category.Name == budget.CategoryName);
                if (existingBudget ==null)
                {
                    Category category = context.Categories.First(x => x.Name == budget.CategoryName);
                    var newBudget = new Budget
                    {
                        Category = category,
                        CategoryId = category.Id,
                        amount = budget.Amount
                    };
                    context.Budgets.Add(newBudget);
                    context.SaveChanges();
                } else
                {
                    existingBudget.amount = budget.Amount;
                    context.SaveChanges();
                }
            }
        }
    }
}
