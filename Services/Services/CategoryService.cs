using IServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Models;
using IRepositories.Interfaces;
using PersonalSpendingAnalysis.Dtos;

namespace Services.Services
{
    public class CategoryService : ICategoryService
    {
        IPersonalSpendingAnalysisRepo repo;

        public CategoryService(IPersonalSpendingAnalysisRepo _repo)
        {
            repo = _repo;
        }

        public List<CategoryModel> GetCategories()
        {
            var dtos = repo.GetCategories().Select(x=> new CategoryModel {
            Id = x.Id, Name= x.Name, SearchString = x.SearchString }).ToList();
            return dtos;
        }

        public List<String> GetListOfCategories()
        {
            var strings = repo.GetCategoryNames();
            return strings;
        }

    }
}
