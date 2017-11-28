using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using PersonalSpendingAnalysis.Models;
using PersonalSpendingAnalysis.Repo;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PersonalSpendingAnalysis.Dialogs
{
    public partial class SyncToWeb : Form
    {
        public SyncToWeb()
        {
            InitializeComponent();
            var userRegistryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("PSAauth");
            if (userRegistryKey != null)
            {
                var username = userRegistryKey.GetValue("User").ToString();
                var password = userRegistryKey.GetValue("Pwd").ToString();
                userRegistryKey.Close();

                this.UsernameTextBox.Text = username;
                this.passwordTextBox.Text = password;
            }
        }

        private void buttonSyncToWeb_Click(object sender, EventArgs e)
        {
            status.Text = "";
            var username = this.UsernameTextBox.Text;
            var password = this.passwordTextBox.Text;

            var userRegistryKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("PSAauth");
            userRegistryKey.SetValue("User", username);
            userRegistryKey.SetValue("Pwd", password);
            userRegistryKey.Close();

            var deleteExistingTransactions = deleteExistingTransactionsCheckbox.Checked;

            

            //LOGIN
            var client = new RestClient("https://www.talkisbetter.com/api/auth");
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", "{\r\n    \"username\":\""+username+"\",\r\n    \"password\":\""+password+"\"\r\n}\r\n", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            if (response.StatusCode!=System.Net.HttpStatusCode.OK)
            {
                status.AppendText(username+"failed to log in\r\n");
                return;
            }
            dynamic loginResult = JsonConvert.DeserializeObject<dynamic>(response.Content);
            var jwt = loginResult.token.Value;
            var userId = (string)loginResult.userId.Value;
            status.AppendText("userId: "+userId+"\r\n");

            var context = new PersonalSpendingAnalysisRepo();
            var categories = context.Categories.ToList();
            var transactions = context.Transaction.Select(x => new TransactionModel
            {
                Id = x.Id,
                amount = x.amount,
                transactionDate = x.transactionDate,
                Notes = x.Notes,
                CategoryId = x.CategoryId,
                SubCategory = x.SubCategory,
                AccountId = x.AccountId,
                SHA256 = x.SHA256,
                userId = userId,
                ManualCategory = x.ManualCategory
            }).ToList();


            Thread backgroundThread = new Thread(
                    new ThreadStart(() =>
                    {

                        //GET TRANSACTIONS
                        client = new RestClient("https://www.talkisbetter.com/api/banks");
                        request = new RestRequest(Method.GET);
                        request.AddHeader("jwt", jwt);
                        request.AddHeader("userId", userId);
                        request.AddHeader("content-type", "application/json");
                        response = client.Execute(request);
                        var myTransactions = JsonConvert.DeserializeObject<dynamic[]>(response.Content);
                        status.BeginInvoke(
                            new Action(() =>
                            {
                                status.AppendText("\r\nsuccessfully read " + myTransactions.Length + " records\r\n");
                            })
                        );

                        var successfullyDeleted = 0;
                        var failedDelete = 0;

                        if (deleteExistingTransactions)
                        {
                            foreach (var transaction in myTransactions)  
                            {
                                client = new RestClient("https://www.talkisbetter.com/api/banks/"+transaction._id);
                                request = new RestRequest(Method.DELETE);
                                request.AddHeader("jwt", jwt);
                                request.AddHeader("userId", userId);
                                request.AddHeader("content-type", "application/json");
                                response = client.Execute(request);
                                if(response.StatusCode==System.Net.HttpStatusCode.OK)
                                {
                                    successfullyDeleted++;
                                    status.BeginInvoke(
                                        new Action(() =>
                                        {
                                            status.AppendText(".");
                                        })
                                    );
                                }
                                else
                                {
                                    failedDelete++;
                                    status.BeginInvoke(
                                        new Action(() =>
                                        {
                                            status.AppendText("x");
                                        })
                                    );

                                }
                            }

                            status.BeginInvoke(
                                new Action(() =>
                                {
                                    status.AppendText("\r\nsuccessfully deleted " + successfullyDeleted + " transactions failed to delete " + failedDelete + " transactions\r\n");
                                })
                            );
                        }

                        




                        //POST EACH NEW TRANSACTION
                        var numberAdded = 0;
                        var numberFailedAdd = 0;
                        client = new RestClient("https://www.talkisbetter.com/api/banks");
                        foreach (var transaction in transactions)  //transactionsToPushUp
                        {
                            var matchingTransaction = myTransactions.SingleOrDefault(x => x.SHA256 == transaction.SHA256);
                            if (deleteExistingTransactions || matchingTransaction == null)
                            {
                                transaction.userId = userId;
                                JsonSerializer serializer = new JsonSerializer();
                                serializer.Converters.Add(new JavaScriptDateTimeConverter());
                                serializer.NullValueHandling = NullValueHandling.Ignore;

                                string jsonTransaction = JsonConvert.SerializeObject(transaction, Formatting.Indented);
                                request = new RestRequest(Method.POST);
                                request.AddHeader("jwt", jwt);
                                request.AddHeader("userId", userId);
                                request.AddHeader("content-type", "application/json");
                                request.AddParameter("application/json", jsonTransaction, ParameterType.RequestBody);
                                response = client.Execute(request);
                                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                                {
                                    numberAdded++;
                                    status.BeginInvoke(
                                        new Action(() =>
                                        {
                                            status.AppendText(".");
                                        })
                                    );
                                }
                                else
                                {
                                    numberFailedAdd++;
                                    status.BeginInvoke(
                                        new Action(() =>
                                        {
                                            status.AppendText("x");
                                        })
                                    );
                                }
                            }
                        }
                        status.BeginInvoke(
                            new Action(() =>
                            {
                                status.AppendText("\r\nsuccessfully added " + numberAdded + " transactions failed to add " + numberFailedAdd + " transactions \r\n");
                            })
                        );

                        //SAVE NEW TRANSACTIONS FROM WEB TO DB
                        numberAdded = 0;
                        numberFailedAdd = 0;
                        foreach (var transaction in myTransactions)  //transactionsToPushUp
                        {
                            string stripped = transaction.ToString();
                            stripped = stripped.Replace("{{", "{").Replace("}}", "}");
                            var t = JsonConvert.DeserializeObject<TransactionModel>(stripped);
                            var matchingTransaction = transactions.SingleOrDefault(x => x.SHA256 == t.SHA256);
                            if (matchingTransaction == null)
                            {
                                var newTransaction = new Repo.Entities.Transaction
                                {
                                    AccountId = t.AccountId,
                                    amount = t.amount,
                                    CategoryId = t.CategoryId,
                                    ManualCategory = t.ManualCategory,
                                    Id = t.Id,
                                    SHA256 = t.SHA256,
                                    transactionDate = t.transactionDate,
                                    Notes = t.Notes,
                                    SubCategory = t.SubCategory
                                };
                                context.Transaction.Add(newTransaction);
                                context.SaveChanges();
                            }
                        }
                        status.BeginInvoke(
                            new Action(() =>
                            {
                                status.AppendText("\r\ndb successfully added " + numberAdded + " transactions failed to add " + numberFailedAdd + " transactions \r\n");
                                status.AppendText("\r\ncompleted transactions \r\n");
                            })
                        );


                        return; //todo remove this when fixing up categories

                        //GET CATEGORIES
                        client = new RestClient("https://www.talkisbetter.com/api/bankcategorys");
                        request = new RestRequest(Method.GET);
                        request.AddHeader("jwt", jwt);
                        request.AddHeader("userId", userId);
                        request.AddHeader("content-type", "application/json");
                        response = client.Execute(request);
                        var myCategorys = JsonConvert.DeserializeObject<List<dynamic>>(response.Content);
                        status.BeginInvoke(
                            new Action(() =>
                            {
                                status.AppendText("\r\nsuccessfully read " + myCategorys.Count + " records\r\n");
                            })
                        );


                        successfullyDeleted = 0;
                        failedDelete = 0;

                        if (deleteExistingTransactions)
                        {
                            foreach (var category in myCategorys)
                            {
                                client = new RestClient("https://www.talkisbetter.com/api/bankcategorys/" + category._id);
                                request = new RestRequest(Method.DELETE);
                                request.AddHeader("jwt", jwt);
                                request.AddHeader("userId", userId);
                                request.AddHeader("content-type", "application/json");
                                response = client.Execute(request);
                                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                                {
                                    successfullyDeleted++;
                                    status.BeginInvoke(
                                        new Action(() =>
                                        {
                                            status.AppendText(".");
                                        })
                                    );
                                }
                                else
                                {
                                    failedDelete++;
                                    status.BeginInvoke(
                                        new Action(() =>
                                        {
                                            status.AppendText("x");
                                        })
                                    );
                                }
                            }

                        }
                        status.BeginInvoke(
                            new Action(() =>
                            {
                                status.AppendText("\r\nsuccessfully deleted " + successfullyDeleted + " categorys failed to delete " + failedDelete + " categorys\r\n");
                            })
                        );


                        //POST EACH NEW TRANSACTION
                        var numberOfCategoriesAdded = 0;
                        var numberFailedAddCategories = 0;
                        client = new RestClient("https://www.talkisbetter.com/api/bankcategorys");
                        foreach (var category in categories)  
                        {
                            var matchingCategory = myCategorys.SingleOrDefault(x => x.Id == category.Id);
                            if (deleteExistingTransactions || matchingCategory == null)
                            {
                                //category.userId = userId;
                                JsonSerializer serializer = new JsonSerializer();
                                serializer.Converters.Add(new JavaScriptDateTimeConverter());
                                serializer.NullValueHandling = NullValueHandling.Ignore;

                                string jsonCategory = JsonConvert.SerializeObject(category, Formatting.Indented);
                                request = new RestRequest(Method.POST);
                                request.AddHeader("jwt", jwt);
                                request.AddHeader("userId", userId);
                                request.AddHeader("content-type", "application/json");
                                request.AddParameter("application/json", jsonCategory, ParameterType.RequestBody);
                                response = client.Execute(request);
                                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                                {
                                    numberOfCategoriesAdded++;
                                    status.BeginInvoke(
                                        new Action(() =>
                                        {
                                            status.AppendText(".");
                                        })
                                    );
                                }
                                else
                                {
                                    numberFailedAddCategories++;
                                    status.BeginInvoke(
                                        new Action(() =>
                                        {
                                            status.AppendText("x");
                                        })
                                    );
                                }
                            }
                        }
                        status.BeginInvoke(
                            new Action(() =>
                            {
                                status.AppendText("\r\nsuccessfully added " + numberOfCategoriesAdded + " categorys failed to add " + numberFailedAddCategories + " categorys \r\n");
                            })
                        );

                    }
            ));
            backgroundThread.Start();


        }
    }
}
