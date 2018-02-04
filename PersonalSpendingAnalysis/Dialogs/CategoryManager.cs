using IServices.Interfaces;
using PersonalSpendingAnalysis.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace PersonalSpendingAnalysis.Dialogs
{
    public partial class CategoryManager : Form
    {
        ICategoryService categoryService;

        public CategoryManager(ICategoryService _categoryService)
        {
            InitializeComponent();
            categoryService = _categoryService;
        }
        
        private void buttonOk_Click(object sender, EventArgs e)
        {

            var originalCategories = categoryService.GetCategories();
            var newCategories = new List<CategoryModel>();
            var updatedCategories = new List<CategoryModel>();
            var unchangedCategories = new List<CategoryModel>();
            var deletedCategories = new List<CategoryModel>();
            var categoriesFromDialog = new List<CategoryModel>();

            //store the data from the dialog in categories list
            foreach (DataGridViewRow row in this.categoriesGridView.Rows)
            {
                Guid id;
                if (row.Cells[0].Value == null)
                {
                    id = Guid.NewGuid();
                }
                else
                {
                    id = (Guid)row.Cells[0].Value;
                }

                String name = (String)row.Cells[1].Value;
                String searchString = (String)row.Cells[2].Value;

                if (name != null && searchString != null)
                {
                    var searchStrings = searchString.Split(',');
                    searchString = String.Join(",", searchStrings.OrderBy(x => x).Distinct().ToArray());
                    categoriesFromDialog.Add(new CategoryModel
                    {
                        Id = id,
                        Name = name,
                        SearchString = searchString
                    });
                }
                
            }

            //todo move this business logic into service so it is more testable
            foreach (var category in categoriesFromDialog.ToArray())
            {
                if (originalCategories.SingleOrDefault(x => x.Id == category.Id && x.Name == category.Name && x.SearchString == category.SearchString)!=null)
                {
                    unchangedCategories.Add(category);
                }
                else if (originalCategories.SingleOrDefault(x => x.Id == category.Id ) !=null )
                {
                    //update the category
                    var existingRowForThisId = originalCategories.SingleOrDefault(x => x.Id == category.Id);
                    existingRowForThisId.Name = category.Name;
                    existingRowForThisId.SearchString = category.SearchString;
                    updatedCategories.Add(category);
                    categoryService.UpdateCategory(category.Id, category.Name, category.SearchString);
                }
                else {
                    //add new category
                    if (!String.IsNullOrEmpty(category.Name))
                    {
                        var newCategory = categoryService.AddNewCategory(new CategoryModel
                        {
                            Id = category.Id,
                            Name = category.Name,
                            SearchString = category.SearchString
                        });
                        newCategories.Add(category);
                    }
                }
                
            }
            
            //deletions
            var futureCategories = new List<CategoryModel>();
            futureCategories.AddRange(newCategories);
            futureCategories.AddRange(updatedCategories);
            futureCategories.AddRange(unchangedCategories);
            deletedCategories = originalCategories;
            foreach (var futureCategory in futureCategories)
            {
                var categoryToDelete = deletedCategories.Single(x=>x.Id == futureCategory.Id);
                deletedCategories.Remove(categoryToDelete);
            }

            //deletedCategories should now have a list of deleted items
            foreach (var deletedCategory in deletedCategories.ToArray())
            {
                categoryService.RemoveCategory(deletedCategory);
            }

        }

    }
}
