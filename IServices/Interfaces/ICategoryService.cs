using PersonalSpendingAnalysis.Models;
using System;
using System.Collections.Generic;

namespace IServices.Interfaces
{
    public interface ICategoryService
    {
        List<CategoryModel> GetCategories();
        List<String> GetListOfCategories();
        CategoryModel AddNewCategory(CategoryModel categoryModel);
        void RemoveCategory(CategoryModel deletedCategory);
        void UpdateCategorySearchString(Guid value, string text);
        void UpdateCategory(Guid id, string name, string searchString);
    }
}
