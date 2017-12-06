using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using PersonalSpendingAnalysis.IServices;
using IServices.Interfaces;

namespace PersonalSpendingAnalysis.Dialogs
{
    
    public partial class Reports : Form
    {
        IQueryService queryService;
        ITransactionService transactionService;
        IReportService reportService;

        public Reports(IQueryService _queryService, ITransactionService _transactionService, IReportService _reportService)
        {
            InitializeComponent();
            queryService = _queryService;
            transactionService = _transactionService;
            reportService = _reportService;
        }

        class SortableTreeNode
        {
            public TreeNode treeNode { get; set; }
            public Decimal? value { get; set; }
        }

        private void runReport()
        {
            treeView.Nodes.Clear();

            var yearList = new List<float>();
            for (var loopYear = this.startDate.Value.Year; loopYear <= this.endDate.Value.Year; loopYear++)
                yearList.Add(loopYear);

            var model = queryService.GetTransactionsWithCategoriesForCharts(this.startDate.Value,this.endDate.Value);
            
            var count = 0;

            foreach (var category in model.CategoryTotals)
            {
                count++;
                decimal percent = (decimal)count / (decimal)model.Transactions.Count();
                UpdateProgressBar(percent);

                var node = new TreeNode(category.CategoryName + " = " + category.Amount);

                //todo make the structure returned from the service be a tree of the right format
                foreach (var year in yearList)
                {
                    var value = model.Transactions.Where(x => x.Category.Name == category.CategoryName && x.transactionDate.Year == year).Sum(x => (Decimal?)x.amount);
                    decimal? permonth = (value / 12.0m);
                    var yearNode = new TreeNode(year + " = " + value + (permonth==null?"":"    (per month = " + ((decimal)permonth).ToString("#.##") + " ) ") );

                    var searchStrings = model.Categories.First(x => x.Name == category.CategoryName).SearchString + ",manually assigned";
                    
                    var subCategories = new List<SortableTreeNode>();

                    foreach (var searchString in searchStrings.Split(',').ToList())
                    {

                        var subCatValue = model.Transactions
                            .Where(x => x.Category.Name == category.CategoryName && x.transactionDate.Year == year && x.SubCategory == searchString)
                            .Sum(x => (Decimal?)x.amount);

                        var subCategoryNode = new TreeNode(searchString + " " + subCatValue);
                        
                        var transactionsInSubcategory = model.Transactions
                            .Where(x => x.Category.Name == category.CategoryName && x.transactionDate.Year == year && x.SubCategory == searchString).ToArray();

                        foreach (var transaction in transactionsInSubcategory)
                        {
                            var transactionString = transaction.transactionDate.ToShortDateString() + " " + transaction.Notes + " " + transaction.amount;
                            var transactionNode = new TreeNode(transactionString);
                            subCategoryNode.Nodes.Add(transactionNode);
                        }
                        
                        subCategories.Add(new SortableTreeNode { treeNode = subCategoryNode, value = subCatValue });

                    }

                    foreach (var subCategory in subCategories.OrderBy(x => x.value))
                    {
                        if (subCategory.treeNode.Nodes.Count > 0)
                            yearNode.Nodes.Add(subCategory.treeNode);
                    }


                    node.Nodes.Add(yearNode);
                }

                treeView.Nodes.Add(node);
            }

            buttonReport.Enabled = true;
            buttonExportPdf.Enabled = true;

        }

        private void UpdateProgressBar(decimal percent)
        {
            progressBar1.Value = (int)percent;
            if (percent==100.0m)
            {
                progressBar1.Visible = false;
            }
        }

        private void buttonReport_Click(object sender, EventArgs e)
        {
            progressBar1.Visible = true;
            buttonReport.Enabled = false;
            buttonExportPdf.Enabled = false;
            runReport();

        }

        private void Reports_Load(object sender, EventArgs e)
        {
            progressBar1.Visible = false;
            var startDate = transactionService.GetEarliestTransactionDate();
            this.endDate.Value = DateTime.Today;
            this.startDate.Value = startDate;
        }

        private void buttonExportPdf_Click(object sender, EventArgs e)
        {
            var exportPdfDlg = new SaveFileDialog();
            exportPdfDlg.Filter = "PDF Files | *.pdf";
            exportPdfDlg.Title = "Enter name of PDF File to export";
            DialogResult result = exportPdfDlg.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                var includeTransactions = this.includeTransactions.Checked;
                reportService.createReport(treeView.Nodes, exportPdfDlg.FileName, includeTransactions);
                Process.Start(exportPdfDlg.FileName);

            }

        }

        
    }
}
