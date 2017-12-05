using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServices.Interfaces
{
    public interface ICategoryService
    {
        List<CategoryModel> GetCategories();
    }
}
