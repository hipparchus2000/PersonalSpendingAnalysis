using IServices.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace PersonalSpendingAnalysis.Dialogs
{
    public partial class ManuallyAssignCategory : Form
    {
        public Guid transactionId { get; set; }
        ITransactionService transactionService;
        ICategoryService categoryService;

        public ManuallyAssignCategory(
            ITransactionService _transactionService,
            ICategoryService _categoryService
            )
        {
            InitializeComponent();
            transactionService = _transactionService;
            categoryService = _categoryService;
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
        


        private void ManuallyAssignCategory_Load(object sender, EventArgs e)
        {
            this.comboBox1.DisplayMember = "Text";
            this.comboBox1.ValueMember = "Value";

            //todo move this to service / repo
            var categories = categoryService.GetCategories().OrderBy(x => x.Name);
            foreach (var category in categories)
            {
                this.comboBox1.Items.Add(new ComboboxItem(category.Name, category.Id));
            }

            var thisTransaction = transactionService.GetTransaction(transactionId);

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
            ComboboxItem item = (ComboboxItem)this.comboBox1.SelectedItem;
            transactionService.UpdateTransactionCategory(transactionId, item.Value, "Manually Set", true);
            this.Close();
        }

        private void buttonResetCategory_Click(object sender, EventArgs e)
        {
            transactionService.UpdateTransactionCategory(transactionId, null, null, false);
            this.Close();
        }
    }
}
