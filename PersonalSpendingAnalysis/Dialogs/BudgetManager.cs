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
    public partial class BudgetManager : Form
    {
        public BudgetManager()
        {
            InitializeComponent();
            //var context = new PersonalSpendingAnalysisRepo();
            //var categories = context.Categories;
            //foreach (var category in categories)
            //{
            //    this.categoriesGridView.Rows.Add(category);
            //}
        }
        

        private void buttonOk_Click(object sender, EventArgs e)
        {
            var context = new PersonalSpendingAnalysisRepo();
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
                var existingRowForThisSHA256 = context.Categories.SingleOrDefault(x => x.Id == id);
                if (existingRowForThisSHA256 == null)
                {
                    if (!String.IsNullOrEmpty(name))
                    {
                        context.Categories.Add(new Repo.Entities.Category
                        {
                            Id = id,
                            Name = name
                        });
                    }
                }
                context.SaveChanges();
                this.Close();
            }
        }

    }
}
