namespace PersonalSpendingAnalysis
{
    partial class PersonalSpendingAnalysis
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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.ImportCsv = new System.Windows.Forms.Button();
            this.manageCategories = new System.Windows.Forms.Button();
            this.ChartSpending = new System.Windows.Forms.Button();
            this.ManageBudget = new System.Windows.Forms.Button();
            this.ManageAccounts = new System.Windows.Forms.Button();
            this.ManageOldImports = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.transactionsGridView = new System.Windows.Forms.DataGridView();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.transactionDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Notes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.amount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Category = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonAutoCategorize = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonCharts = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.transactionsGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1048, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // ImportCsv
            // 
            this.ImportCsv.Location = new System.Drawing.Point(12, 0);
            this.ImportCsv.Name = "ImportCsv";
            this.ImportCsv.Size = new System.Drawing.Size(75, 23);
            this.ImportCsv.TabIndex = 1;
            this.ImportCsv.Text = "Import CSV";
            this.ImportCsv.UseVisualStyleBackColor = true;
            this.ImportCsv.Click += new System.EventHandler(this.ImportCsv_Click);
            // 
            // manageCategories
            // 
            this.manageCategories.Location = new System.Drawing.Point(93, 0);
            this.manageCategories.Name = "manageCategories";
            this.manageCategories.Size = new System.Drawing.Size(115, 23);
            this.manageCategories.TabIndex = 2;
            this.manageCategories.Text = "Manage Categories";
            this.manageCategories.UseVisualStyleBackColor = true;
            this.manageCategories.Click += new System.EventHandler(this.manageCategories_Click);
            // 
            // ChartSpending
            // 
            this.ChartSpending.Location = new System.Drawing.Point(214, 2);
            this.ChartSpending.Name = "ChartSpending";
            this.ChartSpending.Size = new System.Drawing.Size(97, 23);
            this.ChartSpending.TabIndex = 3;
            this.ChartSpending.Text = "Chart Spending";
            this.ChartSpending.UseVisualStyleBackColor = true;
            // 
            // ManageBudget
            // 
            this.ManageBudget.Location = new System.Drawing.Point(317, 2);
            this.ManageBudget.Name = "ManageBudget";
            this.ManageBudget.Size = new System.Drawing.Size(106, 23);
            this.ManageBudget.TabIndex = 4;
            this.ManageBudget.Text = "Manage Budget";
            this.ManageBudget.UseVisualStyleBackColor = true;
            this.ManageBudget.Click += new System.EventHandler(this.ManageBudget_Click);
            // 
            // ManageAccounts
            // 
            this.ManageAccounts.Location = new System.Drawing.Point(429, 2);
            this.ManageAccounts.Name = "ManageAccounts";
            this.ManageAccounts.Size = new System.Drawing.Size(108, 23);
            this.ManageAccounts.TabIndex = 5;
            this.ManageAccounts.Text = "Manage Accounts";
            this.ManageAccounts.UseVisualStyleBackColor = true;
            this.ManageAccounts.Click += new System.EventHandler(this.ManageAccounts_Click);
            // 
            // ManageOldImports
            // 
            this.ManageOldImports.Location = new System.Drawing.Point(543, 2);
            this.ManageOldImports.Name = "ManageOldImports";
            this.ManageOldImports.Size = new System.Drawing.Size(112, 23);
            this.ManageOldImports.TabIndex = 6;
            this.ManageOldImports.Text = "Manage Old Imports";
            this.ManageOldImports.UseVisualStyleBackColor = true;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // transactionsGridView
            // 
            this.transactionsGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.transactionsGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Id,
            this.transactionDate,
            this.Notes,
            this.amount,
            this.Category});
            this.transactionsGridView.Location = new System.Drawing.Point(13, 40);
            this.transactionsGridView.Name = "transactionsGridView";
            this.transactionsGridView.ReadOnly = true;
            this.transactionsGridView.Size = new System.Drawing.Size(1023, 355);
            this.transactionsGridView.TabIndex = 7;
            // 
            // Id
            // 
            this.Id.HeaderText = "Id";
            this.Id.Name = "Id";
            this.Id.ReadOnly = true;
            // 
            // transactionDate
            // 
            this.transactionDate.HeaderText = "Date";
            this.transactionDate.Name = "transactionDate";
            this.transactionDate.ReadOnly = true;
            // 
            // Notes
            // 
            this.Notes.HeaderText = "Desc";
            this.Notes.Name = "Notes";
            this.Notes.ReadOnly = true;
            this.Notes.Width = 300;
            // 
            // amount
            // 
            this.amount.HeaderText = "Amount";
            this.amount.Name = "amount";
            this.amount.ReadOnly = true;
            // 
            // Category
            // 
            this.Category.HeaderText = "Category";
            this.Category.Name = "Category";
            this.Category.ReadOnly = true;
            this.Category.Width = 150;
            // 
            // buttonAutoCategorize
            // 
            this.buttonAutoCategorize.Location = new System.Drawing.Point(939, 401);
            this.buttonAutoCategorize.Name = "buttonAutoCategorize";
            this.buttonAutoCategorize.Size = new System.Drawing.Size(97, 23);
            this.buttonAutoCategorize.TabIndex = 8;
            this.buttonAutoCategorize.Text = "Auto Categorize";
            this.buttonAutoCategorize.UseVisualStyleBackColor = true;
            this.buttonAutoCategorize.Click += new System.EventHandler(this.buttonAutoCategorize_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(647, 401);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(137, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "uncategorized transactions:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // buttonCharts
            // 
            this.buttonCharts.Location = new System.Drawing.Point(661, 2);
            this.buttonCharts.Name = "buttonCharts";
            this.buttonCharts.Size = new System.Drawing.Size(112, 23);
            this.buttonCharts.TabIndex = 10;
            this.buttonCharts.Text = "Charts";
            this.buttonCharts.UseVisualStyleBackColor = true;
            this.buttonCharts.Click += new System.EventHandler(this.buttonCharts_Click);
            // 
            // PersonalSpendingAnalysis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1048, 430);
            this.Controls.Add(this.buttonCharts);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonAutoCategorize);
            this.Controls.Add(this.transactionsGridView);
            this.Controls.Add(this.ManageOldImports);
            this.Controls.Add(this.ManageAccounts);
            this.Controls.Add(this.ManageBudget);
            this.Controls.Add(this.ChartSpending);
            this.Controls.Add(this.manageCategories);
            this.Controls.Add(this.ImportCsv);
            this.Controls.Add(this.toolStrip1);
            this.Name = "PersonalSpendingAnalysis";
            this.Text = "Personal Spending Analysis";
            this.Load += new System.EventHandler(this.PersonalSpendingAnalysis_Load);
            ((System.ComponentModel.ISupportInitialize)(this.transactionsGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.Button ImportCsv;
        private System.Windows.Forms.Button manageCategories;
        private System.Windows.Forms.Button ChartSpending;
        private System.Windows.Forms.Button ManageBudget;
        private System.Windows.Forms.Button ManageAccounts;
        private System.Windows.Forms.Button ManageOldImports;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.DataGridView transactionsGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn transactionDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn Notes;
        private System.Windows.Forms.DataGridViewTextBoxColumn amount;
        private System.Windows.Forms.DataGridViewTextBoxColumn Category;
        private System.Windows.Forms.Button buttonAutoCategorize;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonCharts;
    }
}

