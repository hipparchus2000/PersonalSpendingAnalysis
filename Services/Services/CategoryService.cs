using IServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Models;
using IRepositories.Interfaces;

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
            throw new NotImplementedException();
        }
    }
}
