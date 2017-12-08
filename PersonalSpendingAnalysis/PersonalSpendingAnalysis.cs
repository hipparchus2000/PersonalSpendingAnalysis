using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using PersonalSpendingAnalysis.Dialogs;
using System.IO;
using System.Threading;
using PersonalSpendingAnalysis.IServices;
using IServices.Interfaces;
using Enums;

namespace PersonalSpendingAnalysis
{

    public partial class PersonalSpendingAnalysis : Form
    {
        IImportsAndExportService importsAndExportsService;
        IBudgetsService budgetsService;
        IQueryService queriesService;
        ICategoryService categoryService;
        ITransactionService transactionService;
        IReportService reportService;

        orderBy currentOrder = orderBy.transactionDateDescending;

        public PersonalSpendingAnalysis(
                IImportsAndExportService _importsAndExportsService,
                IBudgetsService _budgetsService,
                IQueryService _queriesService,
                ICategoryService _categoryService,
                ITransactionService _transactionService,
                IReportService _reportService
            )
        {
            InitializeComponent();
            importsAndExportsService = _importsAndExportsService;
            budgetsService = _budgetsService;
            queriesService = _queriesService;
            categoryService = _categoryService;
            transactionService = _transactionService;
            reportService = _reportService;
        }

        private void ImportCsv_Click(object sender, EventArgs e)
        {
            var importCsvDialog = new OpenFileDialog();
            importCsvDialog.Filter = "CSV Files | *.csv";
            importCsvDialog.Title = "Select Csv File to import";
            DialogResult result = importCsvDialog.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                var results = importsAndExportsService.ImportFile(importCsvDialog.FileName);
                
                MessageBox.Show("Import finished ( "+ results.importId+" ) "+ "\r" +
                    "number of fields per row = " + results.NumberOfFieldsFound + "\r" +
                    "number of records = " + results.NumberOfRecordsImported + "\r"+
                    "number of duplicates found = "+ results.NumberOfDuplicatesFound + "\r" +
                    "number of new records = " + results.NumberOfNewRecordsFound
                    );
            }

            refresh();
        } //end click

        private void manageCategories_Click(object sender, EventArgs e)
        {
            var dlg = new CategoryManager(categoryService);
            dlg.Show();
        }

        private void ManageBudget_Click(object sender, EventArgs e)
        {
            var dlg = new BudgetManager(queriesService, categoryService, budgetsService);
            dlg.Show();
        }

        private void ManageAccounts_Click(object sender, EventArgs e)
        {
            var dlg = new AccountManager();
            dlg.Show();
        }

        private void transactionBindingSource_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void PersonalSpendingAnalysis_Load(object sender, EventArgs e)
        {
            refresh();
        }
        
        private void buttonAutoCategorize_Click(object sender, EventArgs e)
        {
            var categories = categoryService.GetCategories();
            var datarows = transactionService.GetTransactions();
            var totalCount = datarows.Count();

            Thread backgroundThread = new Thread(
                new ThreadStart(() =>
                {
                    int currentCount = 0;
                 
                    var force = true;

                    foreach (var datarow in datarows)
                    {
                        currentCount++;
                        progressBar1.BeginInvoke(
                            new Action(() =>
                            {
                                progressBar1.Value = (currentCount / totalCount ) * 100;
                            }
                        ));

                        if (datarow.ManualCategory != true)
                        {
                            if (force == true)
                            {
                                datarow.CategoryId = null;
                            }

                            foreach (var category in categories)
                            {
                                if (datarow.CategoryId == null)
                                {
                                    if (!String.IsNullOrEmpty(category.SearchString))
                                    {
                                        var searchStrings = category.SearchString.ToLower().Split(',');
                                        foreach (var searchString in searchStrings)
                                        {
                                            var trimmedSearchString = searchString.TrimStart().TrimEnd();
                                            if (datarow.Notes.ToLower().Contains(trimmedSearchString))
                                            {
                                                datarow.CategoryId = category.Id;
                                                datarow.SubCategory = trimmedSearchString;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            datarow.SubCategory = "manually assigned";
                        }
                    }

                    //todo move categorisation task to service to improve testability
                    foreach(var datarow in datarows)
                    {
                        transactionService.UpdateTransactionCategory(datarow.Id, datarow.CategoryId, datarow.SubCategory);
                    }
                    
                    MessageBox.Show("Autocategorization completed!");
                    progressBar1.BeginInvoke(
                            new Action(() =>
                            {
                                progressBar1.Value = 0;
                                refresh();
                            }
                        ));
                }
            ));

            backgroundThread.Start();

        }

        private void refresh()
        {
            this.transactionsGridView.Rows.Clear();

            var datarows = transactionService.GetTransactions(currentOrder);


            var uncategorized = 0;
            var totalTransactions = 0;
            decimal value = 0;

            foreach (var datarow in datarows)
            {
                //category
                var row = new DataGridViewRow();
                var idCell = new DataGridViewTextBoxCell();
                idCell.Value = datarow.Id;

                var transactionDateCell = new DataGridViewTextBoxCell { Value = datarow.transactionDate };
                var notesCell = new DataGridViewTextBoxCell { Value = datarow.Notes };
                var amountCell = new DataGridViewTextBoxCell { Value = datarow.amount };
                var categoryCell = new DataGridViewTextBoxCell { Value = datarow.Category==null?null:datarow.Category.Name };
                if (datarow.Category == null)
                {
                    uncategorized++;
                    value += datarow.amount;
                }
                totalTransactions++;

                row.Cells.Add(idCell);
                row.Cells.Add(transactionDateCell);
                row.Cells.Add(notesCell);
                row.Cells.Add(amountCell);
                row.Cells.Add(categoryCell);

                this.transactionsGridView.Rows.Add(row);
            }

            this.label1.Text = ("uncategorized transactions: " + uncategorized + "/" + totalTransactions+" value £"+value);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void buttonCharts_Click(object sender, EventArgs e)
        {
            var dlg = new Charts(transactionService,queriesService);
            dlg.Show();
        }

        private void buttonReports_Click(object sender, EventArgs e)
        {
            var dlg = new Reports(queriesService, transactionService, reportService);
            dlg.Show();
        }

        private void transactionsGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                var row = this.transactionsGridView.Rows[e.RowIndex];
                var description = row.Cells[2].Value.ToString();
                var dlg = new AddSearchStringToCategory(categoryService);
                dlg.setSearchString(description);
                dlg.Show();
            }

            if (e.ColumnIndex == 4)
            {
                var row = this.transactionsGridView.Rows[e.RowIndex];
                var id = row.Cells[0].Value.ToString();
                var dlg = new ManuallyAssignCategory(transactionService, categoryService);
                dlg.setTransactionId(new Guid(id));
                dlg.ShowDialog();
                refresh();
            }
        }


        private void buttonExportAllToCsv_Click(object sender, EventArgs e)
        {
            var dlg = new SaveFileDialog();
            dlg.Title = "Enter name of json file to save as";
            var result = dlg.ShowDialog();
            if (result == DialogResult.OK) 
            {
                Thread backgroundThread = new Thread(
                    new ThreadStart(() =>
                    {
                        var json = importsAndExportsService.GetExportableText();
                        File.WriteAllText(dlg.FileName, json);
                        MessageBox.Show("Export completed!");
                    }
                ));
                backgroundThread.Start();
            }

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void buttonWebSync_Click(object sender, EventArgs e)
        {
            var dlg = new SyncToWeb(importsAndExportsService,transactionService,categoryService);
            dlg.ShowDialog();
        }

        private void buttonImportAllFromCsv_Click(object sender, EventArgs e)
        {
            var numberOfDuplicateTransactions = 0;
            var numberOfNewTransactions = 0;
            var numberOfDuplicateCategories = 0;
            var numberOfNewCategories = 0;

            var dlg = new OpenFileDialog();
            dlg.Title = "Enter name of json file to open";
            var result = dlg.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                var fileText = File.ReadAllText(dlg.FileName);

                //todo move this to service/repo

                importsAndExportsService.ImportJson(fileText);

                refresh();

                MessageBox.Show("Import complete "
                    + numberOfDuplicateTransactions + " duplicate transactions "
                    + numberOfNewTransactions + " new transactions "
                    + numberOfDuplicateCategories + " duplicate categories "
                    + numberOfNewCategories + " new categories"
                    );
            }
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }
    }
}
