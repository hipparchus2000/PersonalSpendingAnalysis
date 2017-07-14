using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using PersonalSpendingAnalysis.Repo;


namespace PersonalSpendingAnalysis.Dialogs
{
    public partial class AddSearchStringToCategory : Form
    {
        public AddSearchStringToCategory()
        {
            InitializeComponent();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        public void setSearchString(String searchString)
        {
            this.textBoxSearchString.Text = searchString;
        }

        private void buttonAddSearchStringToCategory_Click(object sender, EventArgs e)
        {
            var context = new PersonalSpendingAnalysisRepo();
            var selectedCategory = (ComboboxItem)this.comboBoxCategory.SelectedItem;
            var category = context.Categories.Single(x => x.Id == selectedCategory.Value);
            category.SearchString += "," + this.textBoxSearchString.Text;
            context.SaveChanges();
            this.Close();
        }

        class ComboboxItem
        {
            public ComboboxItem(String name, Guid value)
            {
                Text = name;
                Value = value;
            }
            public string Text { get; set; }
            public Guid Value { get; set; }
        }

        private void AddSearchStringToCategory_Load(object sender, EventArgs e)
        {
            this.comboBoxCategory.DisplayMember = "Text";
            this.comboBoxCategory.ValueMember = "Value";

            var context = new PersonalSpendingAnalysisRepo();
            var categories = context.Categories.OrderBy(x=>x.Name);
            foreach (var category in categories)
            {
                this.comboBoxCategory.Items.Add(new ComboboxItem(category.Name, category.Id));
            }
        }
    }
}
