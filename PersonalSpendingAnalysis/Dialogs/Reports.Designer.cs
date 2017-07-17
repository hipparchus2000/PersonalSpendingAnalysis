namespace PersonalSpendingAnalysis.Dialogs
{
    partial class Reports
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
            this.startDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.endDate = new System.Windows.Forms.DateTimePicker();
            this.buttonReport = new System.Windows.Forms.Button();
            this.treeView = new System.Windows.Forms.TreeView();
            this.buttonExportPdf = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // startDate
            // 
            this.startDate.Location = new System.Drawing.Point(13, 28);
            this.startDate.Name = "startDate";
            this.startDate.Size = new System.Drawing.Size(200, 20);
            this.startDate.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Start Date";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(226, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "End Date";
            // 
            // endDate
            // 
            this.endDate.Location = new System.Drawing.Point(227, 28);
            this.endDate.Name = "endDate";
            this.endDate.Size = new System.Drawing.Size(200, 20);
            this.endDate.TabIndex = 5;
            // 
            // buttonReport
            // 
            this.buttonReport.Location = new System.Drawing.Point(469, 28);
            this.buttonReport.Name = "buttonReport";
            this.buttonReport.Size = new System.Drawing.Size(75, 23);
            this.buttonReport.TabIndex = 7;
            this.buttonReport.Text = "Run Report";
            this.buttonReport.UseVisualStyleBackColor = true;
            this.buttonReport.Click += new System.EventHandler(this.buttonReport_Click);
            // 
            // treeView
            // 
            this.treeView.Location = new System.Drawing.Point(15, 78);
            this.treeView.Name = "treeView";
            this.treeView.Size = new System.Drawing.Size(1241, 501);
            this.treeView.TabIndex = 8;
            // 
            // buttonExportPdf
            // 
            this.buttonExportPdf.Location = new System.Drawing.Point(623, 28);
            this.buttonExportPdf.Name = "buttonExportPdf";
            this.buttonExportPdf.Size = new System.Drawing.Size(75, 23);
            this.buttonExportPdf.TabIndex = 9;
            this.buttonExportPdf.Text = "Export Pdf";
            this.buttonExportPdf.UseVisualStyleBackColor = true;
            this.buttonExportPdf.Click += new System.EventHandler(this.buttonExportPdf_Click);
            // 
            // Reports
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1268, 591);
            this.Controls.Add(this.buttonExportPdf);
            this.Controls.Add(this.treeView);
            this.Controls.Add(this.buttonReport);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.endDate);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.startDate);
            this.Name = "Reports";
            this.Text = "Reports";
            this.Load += new System.EventHandler(this.Reports_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DateTimePicker startDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker endDate;
        private System.Windows.Forms.Button buttonReport;
        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.Button buttonExportPdf;
    }
}