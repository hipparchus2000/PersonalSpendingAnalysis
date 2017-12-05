using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using PersonalSpendingAnalysis.Dialogs;
using System.IO;
using System.Threading;
using Unity;
using PersonalSpendingAnalysis.IServices;
using Models.Models;
using IServices.Interfaces;

namespace PersonalSpendingAnalysis
{
    enum orderBy
    {
        transactionDateDescending,
        transactionDateAscending,
        amountAscending,
        amountDescending,
        categoryAscending,
        categoryDescending
    }

    public partial class PersonalSpendingAnalysis : Form
    {
        IUnityContainer container = new UnityContainer();
        IImportsAndExportService importsAndExportsService;
        IAggregatesService aggregatesService;
        IQueryService queriesService;

        orderBy currentOrder = orderBy.transactionDateDescending;
        ICategoryService categoryService;
        ITransactionService transactionService;

        public PersonalSpendingAnalysis(
                IImportsAndExportService _importsAndExportsService,
                IAggregatesService _aggregatesService,
                IQueryService _queriesService
            )
        {
            InitializeComponent();
            importsAndExportsService = _importsAndExportsService;
            aggregatesService = _aggregatesService;
            queriesService = _queriesService;
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
            var dlg = new CategoryManager();
            dlg.Show();
        }

        private void ManageBudget_Click(object sender, EventArgs e)
        {
            var dlg = new BudgetManager();
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

                    //todo work out how to save changes above
                    context.SaveChanges();
                    
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

            //todo move into service / repo
            var categories = context.Categories;
            var datarows = new List<Repo.Entities.Transaction>().ToList();

            switch (currentOrder) {
                case orderBy.transactionDateDescending:
                    datarows = context.Transaction.Include("Category").OrderByDescending(x => x.transactionDate).ToList();
                    break;
                case orderBy.transactionDateAscending:
                    datarows = context.Transaction.Include("Category").OrderBy(x => x.transactionDate).ToList();
                    break;
                case orderBy.amountAscending:
                    datarows = context.Transaction.Include("Category").OrderBy(x => x.amount).ToList();
                    break;
                case orderBy.amountDescending:
                    datarows = context.Transaction.Include("Category").OrderByDescending(x => x.amount).ToList();
                    break;
                case orderBy.categoryAscending:
                    datarows = context.Transaction.Include("Category").OrderBy(x => x.Category==null? "": x.Category.Name).ToList();
                    break;
                case orderBy.categoryDescending:
                    datarows = context.Transaction.Include("Category").OrderByDescending(x => x.Category == null ? "" : x.Category.Name).ToList();
                    break;
            }

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
            var dlg = new Charts();
            dlg.Show();
        }

        private void buttonReports_Click(object sender, EventArgs e)
        {
            var dlg = new Reports();
            dlg.Show();
        }

        private void transactionsGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                var row = this.transactionsGridView.Rows[e.RowIndex];
                var description = row.Cells[2].Value.ToString();
                var dlg = new AddSearchStringToCategory();
                dlg.setSearchString(description);
                dlg.Show();
            }

            if (e.ColumnIndex == 4)
            {
                var row = this.transactionsGridView.Rows[e.RowIndex];
                var id = row.Cells[0].Value.ToString();
                var dlg = new ManuallyAssignCategory();
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
            var dlg = new SyncToWeb();
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

                var import = JsonConvert.DeserializeObject<ExportableModel>(fileText);

                //do the categories first to avoid foreign key issues
                foreach (var category in import.categories)
                {
                    if (context.Categories.SingleOrDefault(x => x.Id == category.Id) != null)
                    {
                        //this method aggregates search strings - todo perhaps give user the choice to aggregate or
                        //replace or keep search strings.
                        numberOfDuplicateCategories++;
                        var oldCategory = context.Categories.Single(x => x.Id == category.Id);
                        var searchStrings = oldCategory.SearchString.Split(',').ToList();
                        searchStrings.AddRange(category.SearchString.Split(','));
                        var uniqueSearchStrings = searchStrings.Distinct();
                        oldCategory.SearchString = string.Join(",", uniqueSearchStrings);                        
                    }
                    else
                    {
                        context.Categories.Add(category);
                        context.SaveChanges();
                        numberOfNewCategories++;
                    }
                }

                foreach (var transaction in import.transactions)
                {
                    if (context.Transaction.SingleOrDefault(x => x.SHA256 == transaction.SHA256) == null)
                    {
                        transaction.Category = null;
                        //import the transaction
                        context.Transaction.Add(transaction);
                        context.SaveChanges();
                        numberOfNewTransactions++;
                    }
                    else
                    {
                        //transaction is already here
                        numberOfDuplicateTransactions++;
                    }
                }

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
