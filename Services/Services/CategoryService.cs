using IServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using IRepositories.Interfaces;
using PersonalSpendingAnalysis.Models;
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

        public CategoryModel AddNewCategory(CategoryModel model)
        {
            repo.AddNewCategory(new CategoryDto
            {
                Id = model.Id, Name = model.Name, SearchString = model.SearchString
            });
            return model;
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

        public void RemoveCategory(CategoryModel model)
        {
            repo.RemoveCategory(new CategoryDto
            {
                Id = model.Id,
                Name = model.Name,
                SearchString = model.SearchString
            });
        }

        public void UpdateCategorySearchString(Guid id, string text)
        {
            repo.UpdateCategorySearchString(id, text);
        }
    }
}
