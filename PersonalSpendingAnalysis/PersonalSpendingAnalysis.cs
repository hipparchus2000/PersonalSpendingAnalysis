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
using System.Security.Cryptography;
using CsvHelper;
using PersonalSpendingAnalysis.Dialogs;
using Newtonsoft.Json;
using PersonalSpendingAnalysis.Repo.Entities;
using System.IO;
using Newtonsoft.Json.Converters;
using System.Threading;

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
        orderBy currentOrder = orderBy.transactionDateDescending;


        public PersonalSpendingAnalysis()
        {
            InitializeComponent();
        }

        public static String sha256_hash(String value)
        {
            StringBuilder Sb = new StringBuilder();

            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(value));

                foreach (Byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }

        public class ImportResults
        {
            public ImportResults ()
            {
                importId = Guid.NewGuid();
            }
            public Guid importId { get; set; }
            public int NumberOfRecordsImported { get; set; }
            public int NumberOfDuplicatesFound { get; set; }
            public int NumberOfNewRecordsFound { get; set; }
            public int NumberOfFieldsFound { get; set; }
        }

        private void ImportCsv_Click(object sender, EventArgs e)
        {
            ImportResults results = new ImportResults();
            var importCsvDialog = new OpenFileDialog();
            importCsvDialog.Filter = "CSV Files | *.csv";
            importCsvDialog.Title = "Select Csv File to import";
            DialogResult result = importCsvDialog.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                string csvText = System.IO.File.ReadAllText(importCsvDialog.FileName);
                csvText = csvText.Replace("\n", "");
                var importLines = csvText.Split('\r');

                var nonNullLineCount = 0;
                String[] headers;
                foreach (var importLine in importLines)
                {
                    if (importLine != "")
                    {
                        results.NumberOfNewRecordsFound++;

                        if (nonNullLineCount == 0)
                        {
                            headers = purgeCommasInTextFields(importLine).Split(',');
                            results.NumberOfFieldsFound = headers.Length;
                        }
                        else
                        {
                            results.NumberOfRecordsImported++;

                            var columns = purgeCommasInTextFields(importLine).Split(',');
                            var context = new PersonalSpendingAnalysisRepo();
                            var id = sha256_hash(importLine);

                            DateTime tDate;
                            DateTime.TryParse(columns[0], out tDate);
                            decimal tAmount;
                            Decimal.TryParse(columns[3], out tAmount);

                            var existingRowForThisSHA256 = context.Transaction.SingleOrDefault(x => x.SHA256 == id);
                            if (existingRowForThisSHA256 == null)
                            {
                                results.NumberOfNewRecordsFound++;

                                context.Transaction.Add(new Repo.Entities.Transaction
                                {
                                    transactionDate = tDate,
                                    amount = tAmount,
                                    Notes = columns[2].Replace("\"",""),
                                    SHA256 = id,
                                });
                                context.SaveChanges();
                            } else
                            {
                                results.NumberOfDuplicatesFound++;
                            }
                        }
                        nonNullLineCount++;
                    }
                }

                MessageBox.Show("Import finished ( "+ results.importId+" ) "+ "\r" +
                    "number of fields per row = " + results.NumberOfFieldsFound + "\r" +
                    "number of records = " + results.NumberOfRecordsImported + "\r"+
                    "number of duplicates found = "+ results.NumberOfDuplicatesFound + "\r" +
                    "number of new records = " + results.NumberOfNewRecordsFound
                    );


            }

            refresh();
        } //end click

        private String purgeCommasInTextFields(String original)
        {
            String modified = "";
            bool insideQuotes = false;
            foreach (var character in original)
            {
                if (character == '"')
                    insideQuotes = !insideQuotes;
                if (character == ',' && insideQuotes)
                {
                    //do nothing
                } else
                {
                    modified = modified + character;
                }
            }
            return modified;
        }


        //private void ImportCsv_Click2(object sender, EventArgs e)
        //{
        //    DialogResult result = openFileDialog1.ShowDialog();
        //    if (result == DialogResult.OK) // Test result.
        //    {
        //        var csv = new CsvReader(System.IO.File.OpenText(openFileDialog1.FileName));
        //        var myCustomObjects = csv.GetRecords<MyCustomObject>();
        //        foreach (var ob in myCustomObjects) {
        //            var importLine = ob.Date + ob.Type + ob.Description + ob.Value + ob.Balance + ob.Account_Name + ob.Account_Number;
        //            var context = new PersonalSpendingAnalysisRepo();
        //            var id = sha256_hash(importLine);

        //            DateTime tDate;
        //            DateTime.TryParse(ob.Date, out tDate);
        //            decimal tAmount;
        //            Decimal.TryParse(ob.Value, out tAmount);

        //            var existingRowForThisSHA256 = context.Transaction.SingleOrDefault(x => x.SHA256 == id);
        //            if (existingRowForThisSHA256 == null)
        //            {
        //                context.Transaction.Add(new Repo.Entities.Transaction
        //                {
        //                    Id = Guid.NewGuid(),
        //                    transactionDate = tDate,
        //                    amount = tAmount,
        //                    Notes = ob.Description,
        //                    Category = null,
        //                    SHA256 = id,
        //                });
        //                context.SaveChanges();
        //            }

        //            refresh();
        //        }
        //    }
        //}

        private class MyCustomObject
        {
            public String Date { get; set; }
            public String Type { get; set; }
            public String Description { get; set; }
            public String Value { get; set; }
            public String Balance { get; set; }
            public String Account_Name { get; set; }
            public String Account_Number { get; set; }
        }

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
            var context = new PersonalSpendingAnalysisRepo();
            var categories = context.Categories;
            var datarows = context.Transaction;
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

            var context = new PersonalSpendingAnalysisRepo();
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

        public class Exportable
        {
            public List<Transaction> transactions;
            public List<Category> categories;
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
                        var context = new PersonalSpendingAnalysisRepo();
                        var exportable = new Exportable();
                        exportable.transactions = context.Transaction.ToList();
                        exportable.categories = context.Categories.ToList();

                        var settings = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore,
                            DateFormatHandling = DateFormatHandling.IsoDateFormat
                        };

                        string json = JsonConvert.SerializeObject(exportable, settings);
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
            var context = new PersonalSpendingAnalysisRepo();

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
                var import = JsonConvert.DeserializeObject<Exportable>(fileText);

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
