using PersonalSpendingAnalysis.Repo;
using PersonalSpendingAnalysis.Repo.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PersonalSpendingAnalysis.Dialogs
{
    public partial class CategoryManager : Form
    {
        public CategoryManager()
        {
            InitializeComponent();
        }
        

        private void buttonOk_Click(object sender, EventArgs e)
        {
            var context = new PersonalSpendingAnalysisRepo();
            var originalCategories = context.Categories.ToList();

            var newCategories = new List<Repo.Entities.Category>();
            var updatedCategories = new List<Repo.Entities.Category>();
            var unchangedCategories = new List<Repo.Entities.Category>();
            var deletedCategories = new List<Repo.Entities.Category>();
            var categoriesFromDialog = new List<Repo.Entities.Category>();

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

                categoriesFromDialog.Add(new Repo.Entities.Category
                {
                    Id = id,
                    Name = name,
                    SearchString = searchString
                });
            }

            foreach(var category in categoriesFromDialog.ToArray())
            {
                if (originalCategories.Any(x => x.Id == category.Id && x.Name == category.Name && x.SearchString == category.SearchString))
                {
                    unchangedCategories.Add(category);
                }
                else if (originalCategories.Any(x => x.Id == category.Id && (x.Name != category.Name || x.SearchString != category.SearchString)))
                {
                    //update the category
                    var existingRowForThisId = context.Categories.SingleOrDefault(x => x.Id == category.Id);
                    existingRowForThisId.Name = category.Name;
                    existingRowForThisId.SearchString = category.SearchString;
                    updatedCategories.Add(category);
                }
                else {
                    //add new category
                    if (!String.IsNullOrEmpty(category.Name))
                    {
                        var newCategory = context.Categories.Add(new Repo.Entities.Category
                        {
                            Id = category.Id,
                            Name = category.Name,
                            SearchString = category.SearchString
                        });
                        newCategories.Add(category);
                    }
                }
                
            }
            
            context.SaveChanges();

            //deletions
            originalCategories = context.Categories.ToList();
            var futureCategories = new List<Category>();
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
                foreach (var transaction in context.Transaction.Where(x => x.CategoryId == deletedCategory.Id)) {
                    transaction.Category = null;
                }
                context.SaveChanges();

                context.Categories.Remove(deletedCategory);
                context.SaveChanges();
            }

        }

    }
}
