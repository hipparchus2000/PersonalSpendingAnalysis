using IServices.Interfaces;
using PersonalSpendingAnalysis.IServices;
using PersonalSpendingAnalysis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace PersonalSpendingAnalysis.Dialogs
{
    public partial class BudgetManager : Form
    {
        IQueryService queryService;
        ICategoryService categoryService;
        IBudgetsService budgetsService;

        public BudgetManager(IQueryService _queryService, ICategoryService _categoryService, IBudgetsService _budgetsService)
        {
            InitializeComponent();
            queryService = _queryService;
            categoryService = _categoryService;
            budgetsService = _budgetsService;
        }

        private void refresh()
        {
            this.dataGridView1.Rows.Clear();

            var today = DateTime.UtcNow;
            var firstDayOfThisMonth = new DateTime(today.Year, today.Month, 1);
            var sixMonthsBeforeFirstDayOfThisMonth = firstDayOfThisMonth.AddMonths(-6);
    
            var last6MonthCategoryTotals = queryService.GetCategoryTotals(sixMonthsBeforeFirstDayOfThisMonth, firstDayOfThisMonth, false);
            var categoryTotalsForAllTime = queryService.GetCategoryTotalsForAllTime();
            var listOfCategories = categoryService.GetListOfCategories();
            var Budgets = budgetsService.GetBudgets();
            var SpendThisMonth = queryService.GetCategoryTotals(firstDayOfThisMonth, today, false);

            var averageNumberOfDaysPerMonth = 29.53;
            var numberOfMonthsOfRecords = queryService.GetNumberOfDaysOfRecordsInSystem()/ averageNumberOfDaysPerMonth;

            var last6MonthAverages = new List<CategoryMonthlyAverage>();
            var averagesForAllTime = new List<CategoryMonthlyAverage>();

            foreach(var total in last6MonthCategoryTotals)
            {
                last6MonthAverages.Add(new CategoryMonthlyAverage
                {
                    CategoryName = total.CategoryName,
                    Amount = (total.Amount / 6.0m)
                });
            }

            foreach(var total in categoryTotalsForAllTime)
            {
                averagesForAllTime.Add(new CategoryMonthlyAverage
                {
                    CategoryName = total.CategoryName,
                    Amount = (total.Amount / (decimal)numberOfMonthsOfRecords)
                });
            }

            
            foreach (var category in listOfCategories)
            {
                var allTime = averagesForAllTime.FirstOrDefault(x => x.CategoryName == category);
                var last6months = last6MonthAverages.FirstOrDefault(x => x.CategoryName == category);
                var budgeted = Budgets.FirstOrDefault(x => x.CategoryName == category);
                var totalThisMonth = SpendThisMonth.FirstOrDefault(x => x.CategoryName == category);

                var categoryNameCell = new DataGridViewTextBoxCell { Value = category };
                var AverageForAllTime = new DataGridViewTextBoxCell { Value = allTime==null?"0":allTime.Amount.ToString("#.##") };
                var SixMonthAverage = new DataGridViewTextBoxCell { Value = last6months==null?"0": last6months.Amount.ToString("#.##") };
                var Budgeted = new DataGridViewTextBoxCell { Value = budgeted==null?"0":budgeted.Amount.ToString("#.##") };
                var TotalThisMonthCell = new DataGridViewTextBoxCell { Value = totalThisMonth==null?"0":totalThisMonth.Amount.ToString("#.##") };
                
                DataGridViewRow dataGridViewRow = new DataGridViewRow();
                dataGridViewRow.Cells.Add(categoryNameCell);
                dataGridViewRow.Cells.Add(AverageForAllTime);
                dataGridViewRow.Cells.Add(SixMonthAverage);
                dataGridViewRow.Cells.Add(Budgeted);
                dataGridViewRow.Cells.Add(TotalThisMonthCell);

                this.dataGridView1.Rows.Add(dataGridViewRow);
                dataGridViewRow.Cells[0].ReadOnly = true;
                dataGridViewRow.Cells[1].ReadOnly = true;
                dataGridViewRow.Cells[2].ReadOnly = true;
                dataGridViewRow.Cells[4].ReadOnly = true;

            }


        }

        private void buttonCopy6MonthToBudget_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in this.dataGridView1.Rows)
            {
                row.Cells[3].Value = row.Cells[2].Value;
            }

        }

        private void buttonSaveBudget_Click(object sender, EventArgs e)
        {
            var listOfBudgets = new List<BudgetModel>();
            foreach (DataGridViewRow row in this.dataGridView1.Rows) {
                if (!String.IsNullOrEmpty((String)row.Cells[0].Value))
                {
                    Decimal amount = 0;
                     Decimal.TryParse((String)row.Cells[3].Value, out amount);
                    listOfBudgets.Add(new BudgetModel
                    {
                        CategoryName = (String)row.Cells[0].Value,
                        Amount = amount
                    });
                }
            }

            budgetsService.CreateOrUpdateBudgets(listOfBudgets);

        }

        private void BudgetManager_Load(object sender, EventArgs e)
        {
            refresh();
            updateNetBudget();
        }

        private void updateNetBudget()
        {
            var listOfBudgets = new List<BudgetModel>();
            foreach (DataGridViewRow row in this.dataGridView1.Rows)
            {
                if (!String.IsNullOrEmpty((String)row.Cells[0].Value))
                {
                    Decimal amount = 0;
                    Decimal.TryParse((String)row.Cells[3].Value, out amount);
                    listOfBudgets.Add(new BudgetModel
                    {
                        CategoryName = (String)row.Cells[0].Value,
                        Amount = amount
                    });
                }
            }

            var total = listOfBudgets.Sum(x => x.Amount);
            netBudget.Text = "Net Budget = " + total;
        }
        
        private void dataGridView1_CellEditEnd(object sender, DataGridViewCellEventArgs e)
        {
            updateNetBudget();
        }
    }
}
