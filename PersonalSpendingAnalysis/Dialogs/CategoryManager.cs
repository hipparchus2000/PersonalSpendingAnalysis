using PersonalSpendingAnalysis.Repo;
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

            foreach (var transaction in context.Transaction)
            {
                transaction.Category=null;
            }
            context.SaveChanges();

            foreach (var category in context.Categories) {
                context.Categories.Remove(category);
            }
            context.SaveChanges();

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

                var existingRowForThisId = context.Categories.SingleOrDefault(x => x.Id == id);
                if (existingRowForThisId == null)
                {
                    if (!String.IsNullOrEmpty(name))
                    {
                        context.Categories.Add(new Repo.Entities.Category
                        {
                            Id = id,
                            Name = name,
                            SearchString = searchString
                        });
                    }
                } else
                {
                    existingRowForThisId.Name = name;
                    existingRowForThisId.SearchString = searchString;
                }
                context.SaveChanges();
                
            }
        }

    }
}
