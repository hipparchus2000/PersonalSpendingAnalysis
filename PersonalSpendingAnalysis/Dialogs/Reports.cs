using PersonalSpendingAnalysis.Repo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Globalization;
using System;
using System.Diagnostics;
using System.IO;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using PdfSharp.Drawing.Layout;
using System.Threading;

namespace PersonalSpendingAnalysis.Dialogs
{
    
    public partial class Reports : Form
    {
        
        public Reports()
        {
            InitializeComponent();
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

            var context = new PersonalSpendingAnalysisRepo();
            var transactions = context.Transaction.Include("Category")
                .Where(x => (x.transactionDate > this.startDate.Value)
                && (x.transactionDate < this.endDate.Value)
                );
            var categories = transactions
                .GroupBy(x => new { CategoryName = x.Category.Name })
                .Select(x => new
                {
                    CategoryName = x.Key.CategoryName,
                    Amount = x.Sum(y => y.amount)
                }).OrderByDescending(x => x.Amount)
                    .ToList();

            var count = 0;

            foreach (var category in categories)
            {
                count++;
                decimal percent = (decimal)count / (decimal)transactions.Count();
                UpdateProgressBar(percent);

                var node = new TreeNode(category.CategoryName + " = " + category.Amount);

                foreach (var year in yearList)
                {
                    var value = transactions.Where(x => x.Category.Name == category.CategoryName && x.transactionDate.Year == year).Sum(x => (Decimal?)x.amount);
                    var yearNode = new TreeNode(year + " = " + value);

                    var searchStrings = context.Categories.First(x => x.Name == category.CategoryName).SearchString + ",manually assigned";


                    var subCategories = new List<SortableTreeNode>();

                    foreach (var searchString in searchStrings.Split(',').ToList())
                    {

                        var subCatValue = transactions
                            .Where(x => x.Category.Name == category.CategoryName && x.transactionDate.Year == year && x.SubCategory == searchString)
                            .Sum(x => (Decimal?)x.amount);

                        var subCategoryNode = new TreeNode(searchString + " " + subCatValue);

                        var transactionsInSubcategory = transactions
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
            //Thread thread = new Thread(runReport);
            //thread.IsBackground = true;
            //thread.Start();
            //need to separate work from GUI for this.
            runReport();

        }

        private void Reports_Load(object sender, EventArgs e)
        {
            progressBar1.Visible = false;

            var context = new PersonalSpendingAnalysisRepo();
            var startDate = context.Transaction.Min(x => x.transactionDate);
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
                makePdf(treeView.Nodes, exportPdfDlg.FileName);
            }
                
        }


        private void makePdf(TreeNodeCollection nodes, string filename)
        {
            List<string> s = new List<string>();
            string spaces = "";
            walkTree(nodes, spaces, ref s);

            var pagesize = 70;
            var numberOfPages = (int)(s.Count / pagesize) +1;

            PdfDocument document = new PdfDocument();
            XFont font = new XFont("Arial", 8, XFontStyle.Regular);

            for (var pageNum = 0; pageNum < numberOfPages; pageNum++)
            {
                var numberOfRows = pagesize;
                if (((pageNum * pagesize)+ pagesize) > s.Count)
                    numberOfRows = s.Count % pagesize;
                string text = String.Join("\r\n", s.GetRange(pageNum* pagesize, numberOfRows));

                PdfPage page = document.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(page);
                XTextFormatter tf = new XTextFormatter(gfx);

                XRect rect = new XRect(20, 50, 550, 850);
                gfx.DrawRectangle(XBrushes.White, rect);
                tf.Alignment = XParagraphAlignment.Left;
                tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);
            }

            document.Save(filename);
            Process.Start(filename);
     
        }

        private void walkTree(TreeNodeCollection nodes, string spaces, ref List<string> s)
        {
            spaces = spaces + "    ";

            foreach(TreeNode node in nodes)
            {
                s.Add(spaces+node.Text);
                if (node.Nodes.Count>0)
                    walkTree(node.Nodes, spaces, ref s);
            }
        }
    }
}
