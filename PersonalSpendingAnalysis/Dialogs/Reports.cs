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

        public class displayModel
        {
            public displayModel ()
            {
                categories = new List<displayCategory>();
            }

            public List<displayCategory> categories { get; set; }
        }

        public class displayCategory
        {
            public displayCategory ()
            {
                yearResults = new List<displayCategoryYearResults>();
            }
            public string name { get; set; }
            public decimal amount { get; set; }
            public List<displayCategoryYearResults> yearResults { get; set; }
        }

        public class displayCategoryYearResults
        {
            public displayCategoryYearResults()
            {
                subcategories = new List<displaySubcategory>();
            }
            public string name { get; set; }
            public decimal amount { get; set; }
            public List<displaySubcategory> subcategories { get; set; }
        }

        public class displaySubcategory
        {
            public displaySubcategory () {
                transactions = new List<displayTransaction>();
                }
            public string name { get; set; }
            public decimal amount { get; set; }
            public List<displayTransaction> transactions { get; set; }
        }

        public class displayTransaction
        {
            public string text { get; set; }
            public string amount { get; set; }
        }


        private void runReport()
        {
            treeView.Nodes.Clear();
            
            var yearList = new List<float>();
            for (var loopYear = this.startDate.Value.Year; loopYear <= this.endDate.Value.Year; loopYear++)
                yearList.Add(loopYear);

            UpdateProgressBar(2);
            var model = queryService.GetTransactionsWithCategoriesForCharts(this.startDate.Value,this.endDate.Value);
            UpdateProgressBar(50);
            
            var displayModel = new displayModel();
            displayModel.categories = model.CategoryTotals.Select(x => new displayCategory { name = x.CategoryName, amount = x.Amount }).ToList();
            foreach (var category in displayModel.categories)
            {
                    category.yearResults = model.Transactions.Where(x => x.Category.Name == category.name  ).
                        Select(y=> new { category = y.Category.Name, amount = y.amount, year = y.transactionDate.Year  } )
                        .GroupBy(q => new { year = q.year, category = q.category })
                        .Select(z=>new displayCategoryYearResults{ name = z.Key.year.ToString(), amount=z.Sum(t=>t.amount) })
                        .ToList();
                    foreach(var yearResult in category.yearResults)
                    {
                        yearResult.subcategories = model.Transactions
                            .Where(x => x.Category.Name == category.name && x.transactionDate.Year == Convert.ToInt32(yearResult.name))
                            .GroupBy(q => new { subcatname = q.SubCategory })
                            .Select(z=>new displaySubcategory { name = z.Key.subcatname , amount = z.Sum(t=>t.amount)})
                            .ToList();
                        foreach (var subcat in yearResult.subcategories)
                        {
                            subcat.transactions = model.Transactions
                            .Where(x => x.Category.Name == category.name && x.transactionDate.Year == Convert.ToInt32(yearResult.name) && x.SubCategory ==subcat.name)
                            .Select(z => new displayTransaction { text = z.transactionDate.ToShortDateString()+" "+z.Notes, amount = z.amount.ToString() })
                            .ToList();
                        }
                    }
            
            }

            foreach (var category in displayModel.categories.OrderBy(x=>x.name))
            {
                UpdateProgressBar(100.0m);

                var node = new TreeNode(category.name + " = " + category.amount);

                foreach (var year in category.yearResults.OrderByDescending(x => x.name))
                {
                    var yearNode = new TreeNode(year.name+" "+year.amount);
                    var subCategories = new List<TreeNode>();

                    foreach (var subcat in year.subcategories.OrderBy(x => x.amount))
                    {
                        var subCategoryNode = new TreeNode(subcat.name+" "+subcat.amount);
                        foreach (var transaction in subcat.transactions)
                        {
                            var transactionNode = new TreeNode(transaction.text+" "+transaction.amount);
                            subCategoryNode.Nodes.Add(transactionNode);
                        }                        
                        subCategories.Add(subCategoryNode);

                    }

                    foreach (var subCategory in subCategories)
                    {
                        if (subCategory.Nodes.Count > 0)
                            yearNode.Nodes.Add(subCategory);
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
                reportService.createReport(treeView.Nodes, exportPdfDlg.FileName, true);
                Process.Start(exportPdfDlg.FileName);

            }

        }

        
    }
}
