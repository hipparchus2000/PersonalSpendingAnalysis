using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonalSpendingAnalysis.Models;
using PersonalSpendingAnalysis.Repo;
using PersonalSpendingAnalysis.Repo.Entities;

namespace PersonalSpendingAnalysis.Services
{
    class Aggregates
    {
        internal static void CreateOrUpdateBudgets(List<BudgetModel> listOfBudgets)
        {
            var context = new PersonalSpendingAnalysisRepo();
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
