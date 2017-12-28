using PersonalSpendingAnalysis.Dtos;
using System;
using System.Collections.Generic;

namespace PersonalSpendingAnalysis.Repo
{
    public interface ICategoryRepo
    {
        List<CategoryDto> GetCategories();
        List<string> GetCategoryNames();
        List<CategoryTotalDto> GetCategoryTotals(DateTime startDate, DateTime endDate, bool showDebitsOnly);
        List<CategoryTotalDto> GetCategoryTotalsForAllTime();
        void AddNewCategory(CategoryDto dto);
        void RemoveCategory(CategoryDto dto);
        void UpdateCategorySearchString(Guid id, string text);
    }
}