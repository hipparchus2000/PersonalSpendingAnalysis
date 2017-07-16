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
    public partial class ManuallyAssignCategory : Form
    {
        public Guid transactionId { get; set; }

        public ManuallyAssignCategory()
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
        

        //private void InitializeComponent()
        //{
        //    this.SuspendLayout();
            // 
            // ManuallyAssignCategory
            // 
        //    this.ClientSize = new System.Drawing.Size(284, 261);
        //    this.Name = "ManuallyAssignCategory";
        //    this.ResumeLayout(false);

        //}

        private void ManuallyAssignCategory_Load(object sender, EventArgs e)
        {
            this.comboBox1.DisplayMember = "Text";
            this.comboBox1.ValueMember = "Value";

            var context = new PersonalSpendingAnalysisRepo();
            var categories = context.Categories.OrderBy(x => x.Name);
            foreach (var category in categories)
            {
                this.comboBox1.Items.Add(new ComboboxItem(category.Name, category.Id));
            }

            var thisTransaction = context.Transaction.Single(x => x.Id == transactionId);

            if (thisTransaction.CategoryId != null) {
                this.comboBox1.SelectedValue = (object)thisTransaction.CategoryId;
            }

        }

        internal void setTransactionId(Guid id)
        {
            transactionId = id;
            this.label4.Text = this.label4.Text + " " + id;
        }

        private void buttonSetCategory_Click(object sender, EventArgs e)
        {
            var context = new PersonalSpendingAnalysisRepo();
            var thisTransaction = context.Transaction.Single(x => x.Id == transactionId);
            thisTransaction.ManualCategory = true;
            ComboboxItem item = (ComboboxItem)this.comboBox1.SelectedItem;
            thisTransaction.CategoryId = item.Value;
            context.SaveChanges();
            this.Close();
        }

        private void buttonResetCategory_Click(object sender, EventArgs e)
        {
            var context = new PersonalSpendingAnalysisRepo();
            var thisTransaction = context.Transaction.Single(x => x.Id == transactionId);
            thisTransaction.ManualCategory = false;
            thisTransaction.CategoryId = null;
            context.SaveChanges();
            this.Close();
        }
    }
}
