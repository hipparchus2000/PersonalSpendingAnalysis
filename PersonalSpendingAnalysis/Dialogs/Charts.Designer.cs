﻿namespace PersonalSpendingAnalysis.Dialogs
{
    partial class Charts
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
            this.chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.startDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.endDate = new System.Windows.Forms.DateTimePicker();
            this.showDebitsOnly = new System.Windows.Forms.CheckBox();
            this.chartType = new System.Windows.Forms.GroupBox();
            this.chartType3 = new System.Windows.Forms.RadioButton();
            this.chartType2 = new System.Windows.Forms.RadioButton();
            this.chartType1 = new System.Windows.Forms.RadioButton();
            this.radioButtonStackedArea = new System.Windows.Forms.RadioButton();
            this.radioButtonStackedColumn = new System.Windows.Forms.RadioButton();
            this.styleBox = new System.Windows.Forms.GroupBox();
            this.buttonBackwards = new System.Windows.Forms.Button();
            this.buttonForward = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.chart)).BeginInit();
            this.chartType.SuspendLayout();
            this.styleBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // chart
            // 
            this.chart.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuBar;
            chartArea1.Name = "ChartArea1";
            this.chart.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chart.Legends.Add(legend1);
            this.chart.Location = new System.Drawing.Point(232, 14);
            this.chart.Name = "chart";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart.Series.Add(series1);
            this.chart.Size = new System.Drawing.Size(1024, 565);
            this.chart.TabIndex = 0;
            this.chart.Text = "chart1";
            title1.Name = "Title";
            title1.Text = "Personal Spending Analysis";
            this.chart.Titles.Add(title1);
            this.chart.Click += new System.EventHandler(this.chart_Click);
            // 
            // startDate
            // 
            this.startDate.Location = new System.Drawing.Point(15, 73);
            this.startDate.Name = "startDate";
            this.startDate.Size = new System.Drawing.Size(200, 20);
            this.startDate.TabIndex = 3;
            this.startDate.ValueChanged += new System.EventHandler(this.startDate_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Start Date";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 117);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "End Date";
            // 
            // endDate
            // 
            this.endDate.Location = new System.Drawing.Point(17, 136);
            this.endDate.Name = "endDate";
            this.endDate.Size = new System.Drawing.Size(200, 20);
            this.endDate.TabIndex = 5;
            this.endDate.ValueChanged += new System.EventHandler(this.endDate_ValueChanged);
            // 
            // showDebitsOnly
            // 
            this.showDebitsOnly.AutoSize = true;
            this.showDebitsOnly.Location = new System.Drawing.Point(17, 200);
            this.showDebitsOnly.Name = "showDebitsOnly";
            this.showDebitsOnly.Size = new System.Drawing.Size(110, 17);
            this.showDebitsOnly.TabIndex = 7;
            this.showDebitsOnly.Text = "Show Debits Only";
            this.showDebitsOnly.UseVisualStyleBackColor = true;
            this.showDebitsOnly.CheckedChanged += new System.EventHandler(this.showDebitsOnly_CheckedChanged);
            // 
            // chartType
            // 
            this.chartType.Controls.Add(this.chartType3);
            this.chartType.Controls.Add(this.chartType2);
            this.chartType.Controls.Add(this.chartType1);
            this.chartType.Location = new System.Drawing.Point(17, 354);
            this.chartType.Name = "chartType";
            this.chartType.Size = new System.Drawing.Size(200, 94);
            this.chartType.TabIndex = 8;
            this.chartType.TabStop = false;
            this.chartType.Text = "Chart Type";
            // 
            // chartType3
            // 
            this.chartType3.AutoSize = true;
            this.chartType3.Location = new System.Drawing.Point(6, 65);
            this.chartType3.Name = "chartType3";
            this.chartType3.Size = new System.Drawing.Size(66, 17);
            this.chartType3.TabIndex = 2;
            this.chartType3.Text = "pie chart";
            this.chartType3.UseVisualStyleBackColor = true;
            this.chartType3.CheckedChanged += new System.EventHandler(this.chartType3_CheckedChanged);
            // 
            // chartType2
            // 
            this.chartType2.AutoSize = true;
            this.chartType2.Location = new System.Drawing.Point(6, 42);
            this.chartType2.Name = "chartType2";
            this.chartType2.Size = new System.Drawing.Size(108, 17);
            this.chartType2.TabIndex = 1;
            this.chartType2.Text = "years by category";
            this.chartType2.UseVisualStyleBackColor = true;
            this.chartType2.CheckedChanged += new System.EventHandler(this.chartType2_CheckedChanged);
            // 
            // chartType1
            // 
            this.chartType1.AutoSize = true;
            this.chartType1.Checked = true;
            this.chartType1.Location = new System.Drawing.Point(6, 19);
            this.chartType1.Name = "chartType1";
            this.chartType1.Size = new System.Drawing.Size(117, 17);
            this.chartType1.TabIndex = 0;
            this.chartType1.TabStop = true;
            this.chartType1.Text = "months by category";
            this.chartType1.UseVisualStyleBackColor = true;
            this.chartType1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // radioButtonStackedArea
            // 
            this.radioButtonStackedArea.AutoSize = true;
            this.radioButtonStackedArea.Location = new System.Drawing.Point(6, 42);
            this.radioButtonStackedArea.Name = "radioButtonStackedArea";
            this.radioButtonStackedArea.Size = new System.Drawing.Size(87, 17);
            this.radioButtonStackedArea.TabIndex = 1;
            this.radioButtonStackedArea.Text = "stacked area";
            this.radioButtonStackedArea.UseVisualStyleBackColor = true;
            this.radioButtonStackedArea.CheckedChanged += new System.EventHandler(this.radioButtonStackedArea_CheckedChanged);
            // 
            // radioButtonStackedColumn
            // 
            this.radioButtonStackedColumn.AutoSize = true;
            this.radioButtonStackedColumn.Checked = true;
            this.radioButtonStackedColumn.Location = new System.Drawing.Point(6, 19);
            this.radioButtonStackedColumn.Name = "radioButtonStackedColumn";
            this.radioButtonStackedColumn.Size = new System.Drawing.Size(100, 17);
            this.radioButtonStackedColumn.TabIndex = 0;
            this.radioButtonStackedColumn.TabStop = true;
            this.radioButtonStackedColumn.Text = "stacked column";
            this.radioButtonStackedColumn.UseVisualStyleBackColor = true;
            this.radioButtonStackedColumn.CheckedChanged += new System.EventHandler(this.radioButtonStackedColumn_CheckedChanged);
            // 
            // styleBox
            // 
            this.styleBox.Controls.Add(this.radioButtonStackedArea);
            this.styleBox.Controls.Add(this.radioButtonStackedColumn);
            this.styleBox.Location = new System.Drawing.Point(19, 482);
            this.styleBox.Name = "styleBox";
            this.styleBox.Size = new System.Drawing.Size(200, 77);
            this.styleBox.TabIndex = 9;
            this.styleBox.TabStop = false;
            this.styleBox.Text = "Chart Type";
            // 
            // buttonBackwards
            // 
            this.buttonBackwards.Location = new System.Drawing.Point(15, 25);
            this.buttonBackwards.Name = "buttonBackwards";
            this.buttonBackwards.Size = new System.Drawing.Size(75, 23);
            this.buttonBackwards.TabIndex = 10;
            this.buttonBackwards.Text = "<<";
            this.buttonBackwards.UseVisualStyleBackColor = true;
            this.buttonBackwards.Click += new System.EventHandler(this.buttonBackwards_Click);
            // 
            // buttonForward
            // 
            this.buttonForward.Location = new System.Drawing.Point(140, 25);
            this.buttonForward.Name = "buttonForward";
            this.buttonForward.Size = new System.Drawing.Size(75, 23);
            this.buttonForward.TabIndex = 11;
            this.buttonForward.Text = ">>";
            this.buttonForward.UseVisualStyleBackColor = true;
            this.buttonForward.Click += new System.EventHandler(this.buttonForward_Click);
            // 
            // Charts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1268, 591);
            this.Controls.Add(this.buttonForward);
            this.Controls.Add(this.buttonBackwards);
            this.Controls.Add(this.styleBox);
            this.Controls.Add(this.chartType);
            this.Controls.Add(this.showDebitsOnly);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.endDate);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.startDate);
            this.Controls.Add(this.chart);
            this.Name = "Charts";
            this.Text = "Charts";
            this.Load += new System.EventHandler(this.Charts_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chart)).EndInit();
            this.chartType.ResumeLayout(false);
            this.chartType.PerformLayout();
            this.styleBox.ResumeLayout(false);
            this.styleBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chart;
        private System.Windows.Forms.DateTimePicker startDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker endDate;
        private System.Windows.Forms.CheckBox showDebitsOnly;
        private System.Windows.Forms.GroupBox chartType;
        private System.Windows.Forms.RadioButton chartType1;
        private System.Windows.Forms.RadioButton chartType2;
        private System.Windows.Forms.RadioButton chartType3;
        private System.Windows.Forms.RadioButton radioButtonStackedArea;
        private System.Windows.Forms.RadioButton radioButtonStackedColumn;
        private System.Windows.Forms.GroupBox styleBox;
        private System.Windows.Forms.Button buttonBackwards;
        private System.Windows.Forms.Button buttonForward;
    }
}