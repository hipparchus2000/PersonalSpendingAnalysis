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
            // 
            // ManageAccounts
            // 
            this.ManageAccounts.Location = new System.Drawing.Point(429, 2);
            this.ManageAccounts.Name = "ManageAccounts";
            this.ManageAccounts.Size = new System.Drawing.Size(108, 23);
            this.ManageAccounts.TabIndex = 5;
            this.ManageAccounts.Text = "Manage Accounts";
            this.ManageAccounts.UseVisualStyleBackColor = true;
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
            // PersonalSpendingAnalysis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1048, 407);
            this.Controls.Add(this.ManageOldImports);
            this.Controls.Add(this.ManageAccounts);
            this.Controls.Add(this.ManageBudget);
            this.Controls.Add(this.ChartSpending);
            this.Controls.Add(this.manageCategories);
            this.Controls.Add(this.ImportCsv);
            this.Controls.Add(this.toolStrip1);
            this.Name = "PersonalSpendingAnalysis";
            this.Text = "Personal Spending Analysis";
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
    }
}

