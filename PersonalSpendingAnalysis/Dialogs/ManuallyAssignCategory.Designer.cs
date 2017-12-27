namespace PersonalSpendingAnalysis.Dialogs
{
    partial class ManuallyAssignCategory
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
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonSetCategory = new System.Windows.Forms.Button();
            this.buttonResetCategory = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(136, 89);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(47, 89);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Category";
            // 
            // buttonSetCategory
            // 
            this.buttonSetCategory.Location = new System.Drawing.Point(308, 89);
            this.buttonSetCategory.Name = "buttonSetCategory";
            this.buttonSetCategory.Size = new System.Drawing.Size(75, 23);
            this.buttonSetCategory.TabIndex = 2;
            this.buttonSetCategory.Text = "Set";
            this.buttonSetCategory.UseVisualStyleBackColor = true;
            this.buttonSetCategory.Click += new System.EventHandler(this.buttonSetCategory_Click);
            // 
            // buttonResetCategory
            // 
            this.buttonResetCategory.Location = new System.Drawing.Point(415, 89);
            this.buttonResetCategory.Name = "buttonResetCategory";
            this.buttonResetCategory.Size = new System.Drawing.Size(75, 23);
            this.buttonResetCategory.TabIndex = 3;
            this.buttonResetCategory.Text = "Clear";
            this.buttonResetCategory.UseVisualStyleBackColor = true;
            this.buttonResetCategory.Click += new System.EventHandler(this.buttonResetCategory_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(47, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "TransactionId: ";
            // 
            // ManuallyAssignCategory
            // 
            this.ClientSize = new System.Drawing.Size(536, 147);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.buttonResetCategory);
            this.Controls.Add(this.buttonSetCategory);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBox1);
            this.Name = "ManuallyAssignCategory";
            this.Load += new System.EventHandler(this.ManuallyAssignCategory_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        //private System.Windows.Forms.ComboBox comboBoxCategory;
        //private System.Windows.Forms.BindingSource categoryBindingSource;
        //private System.Windows.Forms.TextBox textBoxSearchString;
        //private System.Windows.Forms.Button buttonAddSearchStringToCategory;
        //private System.Windows.Forms.Button buttonCancel;
        //private System.Windows.Forms.Label label1;
        //private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonSetCategory;
        private System.Windows.Forms.Button buttonResetCategory;
        private System.Windows.Forms.Label label4;
    }
}