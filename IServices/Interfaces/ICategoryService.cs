using PersonalSpendingAnalysis.Models;
using System;
using System.Collections.Generic;

namespace IServices.Interfaces
{
    public interface ICategoryService
    {
        List<CategoryModel> GetCategories();
        List<String> GetListOfCategories();
    }
}
