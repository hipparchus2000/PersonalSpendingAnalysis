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
            this.ManageBudget = new System.Windows.Forms.Button();
            this.transactionsGridView = new System.Windows.Forms.DataGridView();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.transactionDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Notes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.amount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Category = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonAutoCategorize = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonCharts = new System.Windows.Forms.Button();
            this.buttonExportAllToCsv = new System.Windows.Forms.Button();
            this.buttonImportAllFromCsv = new System.Windows.Forms.Button();
            this.buttonReports = new System.Windows.Forms.Button();
            this.buttonWebSync = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.transactionsGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1175, 25);
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
            // transactionsGridView
            // 
            this.transactionsGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
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
            this.transactionsGridView.Size = new System.Drawing.Size(1150, 355);
            this.transactionsGridView.TabIndex = 7;
            this.transactionsGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.transactionsGridView_CellContentClick);
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
            this.buttonAutoCategorize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
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
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
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
            this.buttonCharts.Location = new System.Drawing.Point(523, 2);
            this.buttonCharts.Name = "buttonCharts";
            this.buttonCharts.Size = new System.Drawing.Size(112, 23);
            this.buttonCharts.TabIndex = 10;
            this.buttonCharts.Text = "Charts";
            this.buttonCharts.UseVisualStyleBackColor = true;
            this.buttonCharts.Click += new System.EventHandler(this.buttonCharts_Click);
            // 
            // buttonExportAllToCsv
            // 
            this.buttonExportAllToCsv.Location = new System.Drawing.Point(797, 2);
            this.buttonExportAllToCsv.Name = "buttonExportAllToCsv";
            this.buttonExportAllToCsv.Size = new System.Drawing.Size(113, 23);
            this.buttonExportAllToCsv.TabIndex = 11;
            this.buttonExportAllToCsv.Text = "Export To PSA Json";
            this.buttonExportAllToCsv.UseVisualStyleBackColor = true;
            this.buttonExportAllToCsv.Click += new System.EventHandler(this.buttonExportAllToCsv_Click);
            // 
            // buttonImportAllFromCsv
            // 
            this.buttonImportAllFromCsv.Location = new System.Drawing.Point(916, 2);
            this.buttonImportAllFromCsv.Name = "buttonImportAllFromCsv";
            this.buttonImportAllFromCsv.Size = new System.Drawing.Size(120, 23);
            this.buttonImportAllFromCsv.TabIndex = 12;
            this.buttonImportAllFromCsv.Text = "Import From PSA Json";
            this.buttonImportAllFromCsv.UseVisualStyleBackColor = true;
            this.buttonImportAllFromCsv.Click += new System.EventHandler(this.buttonImportAllFromCsv_Click);
            // 
            // buttonReports
            // 
            this.buttonReports.Location = new System.Drawing.Point(641, 2);
            this.buttonReports.Name = "buttonReports";
            this.buttonReports.Size = new System.Drawing.Size(112, 23);
            this.buttonReports.TabIndex = 13;
            this.buttonReports.Text = "Reports";
            this.buttonReports.UseVisualStyleBackColor = true;
            this.buttonReports.Click += new System.EventHandler(this.buttonReports_Click);
            // 
            // buttonWebSync
            // 
            this.buttonWebSync.Location = new System.Drawing.Point(1043, 2);
            this.buttonWebSync.Name = "buttonWebSync";
            this.buttonWebSync.Size = new System.Drawing.Size(120, 23);
            this.buttonWebSync.TabIndex = 14;
            this.buttonWebSync.Text = "Web Sync";
            this.buttonWebSync.UseVisualStyleBackColor = true;
            this.buttonWebSync.Click += new System.EventHandler(this.buttonWebSync_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.progressBar1.Location = new System.Drawing.Point(13, 400);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(536, 23);
            this.progressBar1.TabIndex = 15;
            this.progressBar1.Click += new System.EventHandler(this.progressBar1_Click);
            // 
            // PersonalSpendingAnalysis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1175, 430);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.buttonWebSync);
            this.Controls.Add(this.buttonReports);
            this.Controls.Add(this.buttonImportAllFromCsv);
            this.Controls.Add(this.buttonExportAllToCsv);
            this.Controls.Add(this.buttonCharts);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonAutoCategorize);
            this.Controls.Add(this.transactionsGridView);
            this.Controls.Add(this.ManageBudget);
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
        private System.Windows.Forms.Button ManageBudget;
        private System.Windows.Forms.DataGridView transactionsGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn transactionDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn Notes;
        private System.Windows.Forms.DataGridViewTextBoxColumn amount;
        private System.Windows.Forms.DataGridViewTextBoxColumn Category;
        private System.Windows.Forms.Button buttonAutoCategorize;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonCharts;
        private System.Windows.Forms.Button buttonExportAllToCsv;
        private System.Windows.Forms.Button buttonImportAllFromCsv;
        private System.Windows.Forms.Button buttonReports;
        private System.Windows.Forms.Button buttonWebSync;
        private System.Windows.Forms.ProgressBar progressBar1;
    }
}

