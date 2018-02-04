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
using System.Collections.Generic;
using RestSharp;
using Newtonsoft.Json.Converters;

namespace PersonalSpendingAnalysis.Services
{


    public class ImportsAndExportService : IImportsAndExportService
    {
        IPersonalSpendingAnalysisRepo repo;
        public string baseUri = "https://www.talkisbetter.com/api/";

        public ImportsAndExportService(IPersonalSpendingAnalysisRepo _repo)
        {
            repo = _repo;
        }


        public ImportResults ImportCsv(string csvText)
        {
            ImportResults results = new ImportResults();
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
                        var sha = sha256_hash(importLine);

                        DateTime tDate;
                        DateTime.TryParse(columns[0], out tDate);
                        decimal tAmount;
                        Decimal.TryParse(columns[3], out tAmount);

                        var existingRowForThisSHA256 = repo.GetTransaction(sha);

                        if (existingRowForThisSHA256 == null)
                        {
                            results.NumberOfNewRecordsFound++;

                            var dto = new TransactionDto
                            {
                                Id = Guid.NewGuid(),
                                transactionDate = tDate,
                                amount = tAmount,
                                Notes = columns[2].Replace("\"", ""),
                                SHA256 = sha,
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
            exportable.transactions = repo.GetTransactions(new DateTime(2000,01,01),DateTime.UtcNow).Select(x=> new TransactionModel
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

        public LoginResult LoginToWebService(string username, string password)
        {
            var result = new LoginResult();
            var client = new RestClient(baseUri+"auth");
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", "{\r\n    \"username\":\"" + username + "\",\r\n    \"password\":\"" + password + "\"\r\n}\r\n", ParameterType.RequestBody);
            var response = client.Execute(request);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                result.success = false;
                return result;
            }
            result.success = true;
            dynamic loginResult = JsonConvert.DeserializeObject<dynamic>(response.Content);
            result.jwt = loginResult.token.Value;
            result.userId = (string)loginResult.userId.Value;
            return result;
        }

        public RemoteCategoryModel GetRemoteCategories(LoginResult loginResult)
        {
            var result = new RemoteCategoryModel();
            var client = new RestClient(baseUri+"bankcategorys");
            var request = new RestRequest(Method.GET);
            request.AddHeader("jwt", loginResult.jwt);
            request.AddHeader("userId", loginResult.userId);
            request.AddHeader("content-type", "application/json");
            var response = client.Execute(request);
            result.Success = response.StatusCode == System.Net.HttpStatusCode.OK;
            result.ErrorMessage = response.ErrorMessage;
            if (result.Success == false) return result;
            result.remoteCategories = JsonConvert.DeserializeObject<List<dynamic>>(response.Content);
            return result;
        }

        public bool DeleteRemoteCategory(LoginResult loginResult, Guid id)
        {
            var client = new RestClient(baseUri+"bankcategorys/" + id);
            var request = new RestRequest(Method.DELETE);
            request.AddHeader("jwt", loginResult.jwt);
            request.AddHeader("userId", loginResult.userId);
            request.AddHeader("content-type", "application/json");
            var response = client.Execute(request);
            var result = response.StatusCode == System.Net.HttpStatusCode.OK;
            return result;
        }

        public bool PostNewCategoryToRemote(LoginResult loginResult, object localCategory)
        {
            var client = new RestClient(baseUri+"bankcategorys");
            JsonSerializer serializer = new JsonSerializer();
            serializer.Converters.Add(new JavaScriptDateTimeConverter());
            serializer.NullValueHandling = NullValueHandling.Ignore;

            string jsonCategory = JsonConvert.SerializeObject(localCategory, Formatting.Indented);
            var request = new RestRequest(Method.POST);
            request.AddHeader("jwt", loginResult.jwt);
            request.AddHeader("userId", loginResult.userId);
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", jsonCategory, ParameterType.RequestBody);
            var response = client.Execute(request);
            return response.StatusCode == System.Net.HttpStatusCode.OK; 
        }

        public RemoteTransactionModel GetRemoteTransactions(LoginResult loginResult)
        {
            var result = new RemoteTransactionModel();
            var client = new RestClient(baseUri+"banks");
            var request = new RestRequest(Method.GET);
            request.AddHeader("jwt", loginResult.jwt);
            request.AddHeader("userId", loginResult.userId);
            request.AddHeader("content-type", "application/json");
            var response = client.Execute(request);
            result.Success = response.StatusCode == System.Net.HttpStatusCode.OK;
            result.ErrorMessage = response.ErrorMessage;
            if (result.Success == false) return result;
            result.remoteTransactions = JsonConvert.DeserializeObject<List<dynamic>>(response.Content);
            return result;
        }

        public bool DeleteRemoteTransaction(LoginResult loginResult, Guid id)
        {
            var client = new RestClient(baseUri+"banks/" + id);
            var request = new RestRequest(Method.DELETE);
            request.AddHeader("jwt", loginResult.jwt);
            request.AddHeader("userId", loginResult.userId);
            request.AddHeader("content-type", "application/json");
            var response = client.Execute(request);
            var result = response.StatusCode == System.Net.HttpStatusCode.OK;
            return result;
        }

        public bool PostNewTransactionToRemote(LoginResult loginResult, object localTransaction)
        {
            var client = new RestClient(baseUri+"banks");
            JsonSerializer serializer = new JsonSerializer();
            serializer.Converters.Add(new JavaScriptDateTimeConverter());
            serializer.NullValueHandling = NullValueHandling.Ignore;

            string jsonTransaction = JsonConvert.SerializeObject(localTransaction, Formatting.Indented);
            var request = new RestRequest(Method.POST);
            request.AddHeader("jwt", loginResult.jwt);
            request.AddHeader("userId", loginResult.userId);
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", jsonTransaction, ParameterType.RequestBody);
            var response = client.Execute(request);
            return response.StatusCode == System.Net.HttpStatusCode.OK;
        }

        public CategoryModel ExtractCategoryModelFromJson(string stripped)
        {
            var t = JsonConvert.DeserializeObject<CategoryModel>(stripped);
            return t;
        }

        public TransactionModel ExtractTransactionModelFromJson(string stripped)
        {
            var t = JsonConvert.DeserializeObject<TransactionModel>(stripped);
            return t;
        }
    }
}
