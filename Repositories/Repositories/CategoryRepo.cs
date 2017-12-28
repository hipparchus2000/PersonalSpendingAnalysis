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
    public class CategoryRepo : ICategoryRepo 
    {
        private PSAContext context;

        public CategoryRepo(PSAContext _context) 
        {
            context = _context;
        }

        public List<CategoryDto> GetCategories()
        {
            return context.Categories.Select(x=>new CategoryDto {
                Id = x.Id,
                Name = x.Name,
                SearchString = x.SearchString
            }).OrderBy(x => x.Name).ToList();
        }

        public List<string> GetCategoryNames()
        {
            var categoryList = context.Categories.Select(x => x.Name).OrderBy(x => x).ToList();
            return categoryList;
        }

        public List<CategoryTotalDto> GetCategoryTotals(DateTime startDate, DateTime endDate, bool showDebitsOnly)
        {
            var transactions = context.Transaction.Include("Category")
                .Where(x => (x.transactionDate > startDate)
                && (x.transactionDate < endDate)
                );

            var categories = transactions
                .GroupBy(x => new { CategoryName = x.Category.Name })
                .Select(x => new CategoryTotalDto
                {
                    CategoryName = x.Key.CategoryName,
                    Amount = -1 * x.Sum(y => y.amount)
                }).OrderByDescending(x => x.Amount)
                    .ToList();
            if (showDebitsOnly)
                categories = categories.Where(x => x.Amount > 0).ToList();
            return categories;
        }

        public List<CategoryTotalDto> GetCategoryTotalsForAllTime()
        {
            var transactions = context.Transaction.Include("Category");
            var categories = transactions
                .GroupBy(x => new { CategoryName = x.Category.Name })
                .Select(x => new CategoryTotalDto
                {
                    CategoryName = x.Key.CategoryName,
                    Amount = -1 * x.Sum(y => y.amount)
                }).OrderByDescending(x => x.Amount)
                    .ToList();
            return categories;
        }

        public void AddNewCategory(CategoryDto dto)
        {
            context.Categories.Add(new Category {
                Id = dto.Id, Name = dto.Name, SearchString = dto.SearchString
            });
        }

        public void RemoveCategory(CategoryDto dto)
        {
            foreach (var transaction in context.Transaction.Where(x => x.CategoryId == dto.Id))
            {
                transaction.Category = null;
            }
            context.SaveChanges();

            var category = context.Categories.Single(x=>x.Id == dto.Id);
            context.Categories.Remove(category);
            context.SaveChanges();
        }

        public void UpdateCategorySearchString(Guid id, string text)
        {
            var category = context.Categories.Single(x => x.Id == id);
            category.SearchString += "," + text;
            context.SaveChanges();
        }

    }
}
