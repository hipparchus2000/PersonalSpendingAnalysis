using System;
using System.Windows.Forms;
using System.Linq;

namespace PersonalSpendingAnalysis.Dialogs
{
    partial class CategoryManager
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.categoriesGridView = new System.Windows.Forms.DataGridView();
            this.buttonOk = new System.Windows.Forms.Button();
            this.Idcol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NameCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SearchString = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.categoriesGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // categoriesGridView
            // 
            this.categoriesGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.categoriesGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Idcol,
            this.NameCol,
            this.SearchString});
            this.categoriesGridView.Location = new System.Drawing.Point(23, 47);
            this.categoriesGridView.Name = "categoriesGridView";
            this.categoriesGridView.Size = new System.Drawing.Size(833, 389);
            this.categoriesGridView.TabIndex = 0;
            // 
            // buttonOk
            // 
            this.buttonOk.Location = new System.Drawing.Point(748, 458);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(108, 23);
            this.buttonOk.TabIndex = 1;
            this.buttonOk.Text = "Save Changes";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // Idcol
            // 
            this.Idcol.HeaderText = "Id";
            this.Idcol.Name = "Idcol";
            this.Idcol.ReadOnly = true;
            this.Idcol.Width = 150;
            // 
            // NameCol
            // 
            this.NameCol.HeaderText = "Name";
            this.NameCol.Name = "NameCol";
            this.NameCol.Width = 300;
            // 
            // SearchString
            // 
            this.SearchString.HeaderText = "SearchString";
            this.SearchString.Name = "SearchString";
            this.SearchString.Width = 300;
            // 
            // CategoryManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(868, 502);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.categoriesGridView);
            this.Name = "CategoryManager";
            this.Text = "CategoryManager";
            this.Load += new System.EventHandler(this.CategoryManager_Load);
            ((System.ComponentModel.ISupportInitialize)(this.categoriesGridView)).EndInit();
            this.ResumeLayout(false);

        }

        private void CategoryManager_Load(object sender, EventArgs e)
        {
            refreshCategories();
        }

        private void refreshCategories()
        {

            var context = new PersonalSpendingAnalysisRepo();
            var categories = context.Categories.OrderBy(x => x.Name);

            foreach (var category in categories)
            {
                //category
                var row = new DataGridViewRow();
                var idCell = new DataGridViewTextBoxCell();
                idCell.Value = category.Id;

                var nameCell = new DataGridViewTextBoxCell();
                nameCell.Value = category.Name;

                var searchStringCell = new DataGridViewTextBoxCell();
                searchStringCell.Value = category.SearchString;

                row.Cells.Add(idCell);
                row.Cells.Add(nameCell);
                row.Cells.Add(searchStringCell);
                this.categoriesGridView.Rows.Add(row);
            }
        }

        #endregion

        private System.Windows.Forms.DataGridView categoriesGridView;
        private System.Windows.Forms.Button buttonOk;
        private DataGridViewTextBoxColumn Idcol;
        private DataGridViewTextBoxColumn NameCol;
        private DataGridViewTextBoxColumn SearchString;
    }
}