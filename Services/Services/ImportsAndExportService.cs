using PersonalSpendingAnalysis.IServices;
using System;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using Newtonsoft.Json;
using IRepositories.Interfaces;
using Enums;
using PersonalSpendingAnalysis.Dtos;
using PersonalSpendingAnalysis.Models;

namespace PersonalSpendingAnalysis.Services
{

    public class ImportsAndExportService : IImportsAndExportService
    {
        IPersonalSpendingAnalysisRepo repo;

        public ImportsAndExportService(IPersonalSpendingAnalysisRepo _repo)
        {
            repo = _repo;
        }


        public ImportResults ImportFile(string fileName)
        {
            ImportResults results = new ImportResults();
            string csvText = System.IO.File.ReadAllText(fileName);
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
                        var id = sha256_hash(importLine);

                        DateTime tDate;
                        DateTime.TryParse(columns[0], out tDate);
                        decimal tAmount;
                        Decimal.TryParse(columns[3], out tAmount);

                        var existingRowForThisSHA256 = repo.GetTransaction(id);

                        if (existingRowForThisSHA256 == null)
                        {
                            results.NumberOfNewRecordsFound++;

                            var dto = new TransactionDto
                            {
                                transactionDate = tDate,
                                amount = tAmount,
                                Notes = columns[2].Replace("\"", ""),
                                SHA256 = id,
                            };

                            repo.AddTransaction(dto);

                        }
                        else
                        {
                            results.NumberOfDuplicatesFound++;
                        }
                    }
                    nonNullLineCount++;
                } //if import line is not empty
            } // foreach import line

            return results;

        } //end of method


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
                }
                else
                {
                    modified = modified + character;
                }
            }
            return modified;
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

        public string GetExportableText()
        {
            var exportable = new ExportableModel();
            exportable.transactions = repo.GetTransactions().Select(x=> new TransactionModel
            {
                AccountId = x.AccountId,
                amount = x.amount,
                CategoryId = x.CategoryId,
                Id = x.Id,
                ManualCategory = x.ManualCategory,
                Notes = x.Notes,
                SHA256 = x.SHA256,
                SubCategory = x.SubCategory,
                transactionDate = x.transactionDate,
                Category = null
            }).ToList();
            exportable.categories = repo.GetCategories().Select(x=>new CategoryModel
            {
                Id = x.Id, Name = x.Name, SearchString = x.SearchString
            }).ToList();

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                DateFormatHandling = DateFormatHandling.IsoDateFormat
            };
            string json = JsonConvert.SerializeObject(exportable, settings);
            return json;

        }

        public ImportResult ImportJson(string fileText)
        {
            var import = JsonConvert.DeserializeObject<ExportableModel>(fileText);
            var exportableDto = new ExportableDto
            {
                categories = import.categories.Select(x=>new CategoryDto {
                    Id = x.Id, Name = x.Name, SearchString = x.SearchString
                }).ToList(),
                transactions = import.transactions.Select(x=>new TransactionDto {
                    AccountId = x.AccountId, amount = x.amount, CategoryId = x.CategoryId, Id = x.Id,
                    ManualCategory = x.ManualCategory, Notes = x.Notes, SHA256 = x.SHA256, SubCategory = x.SubCategory,
                    transactionDate = x.transactionDate, Category = null
                }).ToList()
            };
            return repo.ImportCategoriesAndTransactions(exportableDto);
        }
    }
}
