using IRepositories.Interfaces;
using PersonalSpendingAnalysis.Repo.Entities;
using System.Data.Entity;
using System.Linq;
using System.Collections.Generic;
using Enums;
using PersonalSpendingAnalysis.Dtos;
using System;

namespace PersonalSpendingAnalysis.Repo
{
    public class BudgetRepo : IBudgetRepo
    {
        private PSAContext context;

        public BudgetRepo(PSAContext _context) 
        {
            context = _context;
        }

        public List<BudgetDto> GetBudgets()
        {
            var budgetList = context.Budgets.Select(x => new BudgetDto { CategoryName = x.Category.Name, Amount = x.amount }).OrderBy(x=>x.CategoryName).ToList();
            return budgetList;
        }

        public void CreateOrUpdateBudgets(List<BudgetDto> listOfBudgets)
        {
            foreach (var budget in listOfBudgets)
            {
                var existingBudget = context.Budgets.Include("Category").FirstOrDefault(x => x.Category.Name == budget.CategoryName);
                if (existingBudget == null)
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
                }
                else
                {
                    existingBudget.amount = budget.Amount;
                    context.SaveChanges();
                }
            }
        }

    }
}
