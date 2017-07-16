﻿using PersonalSpendingAnalysis.Repo;
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

        private void buttonReport_Click(object sender, EventArgs e)
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
                .Select(x => new {
                    CategoryName = x.Key.CategoryName,
                    Amount = x.Sum(y => y.amount)
                }).OrderByDescending(x => x.Amount)
                    .ToList();

            foreach (var category in categories)
            {
                var node = new TreeNode(category.CategoryName + " = " + category.Amount);

                foreach (var year in yearList)
                {
                    var value = transactions.Where(x => x.Category.Name == category.CategoryName && x.transactionDate.Year == year).Sum(x => (Decimal?)x.amount );
                    var yearNode = new TreeNode(year + " = " + value);

                    var searchStrings = context.Categories.First(x => x.Name == category.CategoryName).SearchString+",manually assigned";


                    var subCategories = new List<SortableTreeNode>();

                    foreach (var searchString in searchStrings.Split(',').ToList())
                    {
                        var subCatValue = transactions
                            .Where(x => x.Category.Name == category.CategoryName && x.transactionDate.Year == year && x.SubCategory==searchString )
                            .Sum(x => (Decimal?)x.amount);
                        
                        var subCategoryNode = new TreeNode( searchString + " " + subCatValue );
                        
                        var transactionsInSubcategory = transactions
                            .Where(x => x.Category.Name == category.CategoryName && x.transactionDate.Year == year && x.SubCategory == searchString).ToArray();

                        foreach (var transaction in transactionsInSubcategory)
                        {
                            var transactionString = transaction.transactionDate + " " + transaction.Notes + " " + transaction.amount;
                            var transactionNode = new TreeNode(transactionString);
                            subCategoryNode.Nodes.Add(transactionNode);
                        }
                        
                        subCategories.Add(new SortableTreeNode { treeNode = subCategoryNode, value = subCatValue });

                    }

                    foreach (var subCategory in subCategories.OrderBy(x=>x.value))
                    {
                        yearNode.Nodes.Add(subCategory.treeNode);
                    }


                    node.Nodes.Add(yearNode);
                }

                treeView.Nodes.Add(node);

            }



        }

        private void Reports_Load(object sender, EventArgs e)
        {
            var context = new PersonalSpendingAnalysisRepo();
            var startDate = context.Transaction.Min(x => x.transactionDate);
            this.endDate.Value = DateTime.Today;
            this.startDate.Value = startDate;
        }
    }
}