using System;
using System.Windows.Forms;

namespace PersonalSpendingAnalysis.Dialogs
{
    partial class BudgetManager
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.CategoryName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MonthlyAverageAll = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AverageLast6Months = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Budget = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CurrentSpendThisMonth = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonCopy6MonthToBudget = new System.Windows.Forms.Button();
            this.buttonSaveBudget = new System.Windows.Forms.Button();
            this.netBudget = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CategoryName,
            this.MonthlyAverageAll,
            this.AverageLast6Months,
            this.Budget,
            this.CurrentSpendThisMonth});
            this.dataGridView1.Location = new System.Drawing.Point(13, 101);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(1001, 405);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(dataGridView1_CellEditEnd);
            // 
            // CategoryName
            // 
            this.CategoryName.HeaderText = "Category";
            this.CategoryName.Name = "CategoryName";
            // 
            // MonthlyAverageAll
            // 
            this.MonthlyAverageAll.HeaderText = "Monthly Average For All Time";
            this.MonthlyAverageAll.Name = "MonthlyAverageAll";
            // 
            // AverageLast6Months
            // 
            this.AverageLast6Months.HeaderText = "Average Last 6 Months";
            this.AverageLast6Months.Name = "AverageLast6Months";
            // 
            // Budget
            // 
            this.Budget.HeaderText = "Budget";
            this.Budget.Name = "Budget";
            // 
            // CurrentSpendThisMonth
            // 
            this.CurrentSpendThisMonth.HeaderText = "Current Spend This Month";
            this.CurrentSpendThisMonth.Name = "CurrentSpendThisMonth";
            // 
            // buttonCopy6MonthToBudget
            // 
            this.buttonCopy6MonthToBudget.Location = new System.Drawing.Point(13, 40);
            this.buttonCopy6MonthToBudget.Name = "buttonCopy6MonthToBudget";
            this.buttonCopy6MonthToBudget.Size = new System.Drawing.Size(134, 38);
            this.buttonCopy6MonthToBudget.TabIndex = 2;
            this.buttonCopy6MonthToBudget.Text = "Copy 6 Month Average to budget";
            this.buttonCopy6MonthToBudget.UseVisualStyleBackColor = true;
            this.buttonCopy6MonthToBudget.Click += new System.EventHandler(this.buttonCopy6MonthToBudget_Click);
            // 
            // buttonSaveBudget
            // 
            this.buttonSaveBudget.Location = new System.Drawing.Point(169, 40);
            this.buttonSaveBudget.Name = "buttonSaveBudget";
            this.buttonSaveBudget.Size = new System.Drawing.Size(147, 38);
            this.buttonSaveBudget.TabIndex = 3;
            this.buttonSaveBudget.Text = "Save Budget";
            this.buttonSaveBudget.UseVisualStyleBackColor = true;
            this.buttonSaveBudget.Click += new System.EventHandler(this.buttonSaveBudget_Click);
            // 
            // netBudget
            // 
            this.netBudget.AutoSize = true;
            this.netBudget.Location = new System.Drawing.Point(366, 513);
            this.netBudget.Name = "netBudget";
            this.netBudget.Size = new System.Drawing.Size(72, 13);
            this.netBudget.TabIndex = 4;
            this.netBudget.Text = "Net budget = ";
            // 
            // BudgetManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1023, 573);
            this.Controls.Add(this.netBudget);
            this.Controls.Add(this.buttonSaveBudget);
            this.Controls.Add(this.buttonCopy6MonthToBudget);
            this.Controls.Add(this.dataGridView1);
            this.Name = "BudgetManager";
            this.Text = "Budget Manager";
            this.Load += new System.EventHandler(this.BudgetManager_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        
        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button buttonCopy6MonthToBudget;
        private System.Windows.Forms.Button buttonSaveBudget;
        private System.Windows.Forms.DataGridViewTextBoxColumn CategoryName;
        private System.Windows.Forms.DataGridViewTextBoxColumn MonthlyAverageAll;
        private System.Windows.Forms.DataGridViewTextBoxColumn AverageLast6Months;
        private System.Windows.Forms.DataGridViewTextBoxColumn Budget;
        private System.Windows.Forms.DataGridViewTextBoxColumn CurrentSpendThisMonth;
        private System.Windows.Forms.Label netBudget;
    }
}