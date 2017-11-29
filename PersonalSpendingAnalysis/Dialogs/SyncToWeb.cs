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
            var localCategories = context.Categories.Select(x=> new {
                Id = x.Id,
                Name = x.Name,
                SearchString = x.SearchString,
                userId = userId
            }).ToList();
            var localTransactions = context.Transaction.Select(x => new TransactionModel
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
                        //categories first to make sure primary keys are ok
                        //GET CATEGORIES
                        client = new RestClient("https://www.talkisbetter.com/api/bankcategorys");
                        request = new RestRequest(Method.GET);
                        request.AddHeader("jwt", jwt);
                        request.AddHeader("userId", userId);
                        request.AddHeader("content-type", "application/json");
                        response = client.Execute(request);
                        if(response.StatusCode != System.Net.HttpStatusCode.OK)
                        {
                            status.BeginInvoke(
                            new Action(() =>
                            {
                                status.AppendText("\r\nError encountered reading categories " + response.ErrorMessage + " \r\n");
                            })
                        );
                        }
                        var remoteCategories = JsonConvert.DeserializeObject<List<dynamic>>(response.Content);
                        status.BeginInvoke(
                            new Action(() =>
                            {
                                status.AppendText("\r\nsuccessfully downloaded " + remoteCategories.Count + " categories\r\n");
                            })
                        );


                        var successfullyDeleted = 0;
                        var failedDelete = 0;

                        if (deleteExistingTransactions)
                        {
                            foreach (var remoteCategory in remoteCategories)
                            {
                                client = new RestClient("https://www.talkisbetter.com/api/bankcategorys/" + remoteCategory._id);
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
                            status.BeginInvoke(
                                new Action(() =>
                                {
                                    status.AppendText("\r\nsuccessfully deleted " + successfullyDeleted + " remote categories failed to delete " + failedDelete + " remote categories\r\n");
                                })
                            );
                        }


                        //POST EACH NEW CATEGORY
                        var numberOfCategoriesAdded = 0;
                        var numberFailedAddCategories = 0;
                        client = new RestClient("https://www.talkisbetter.com/api/bankcategorys");
                        foreach (var localCategory in localCategories)
                        {
                            var matchingCategory = remoteCategories.SingleOrDefault(x => x.Id == localCategory.Id);
                            if (deleteExistingTransactions || matchingCategory == null)
                            {
                                //no remote category for the localCategory
                                //category.userId = userId;
                                JsonSerializer serializer = new JsonSerializer();
                                serializer.Converters.Add(new JavaScriptDateTimeConverter());
                                serializer.NullValueHandling = NullValueHandling.Ignore;

                                string jsonCategory = JsonConvert.SerializeObject(localCategory, Formatting.Indented);
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
                                } //end of test Http Response
                            }
                            else
                            {
                                //existing remote category for the localCategory
                                //todo merge search strings
                            }
                        }
                        status.BeginInvoke(
                            new Action(() =>
                            {
                                status.AppendText("\r\nsuccessfully uploaded " + numberOfCategoriesAdded + " categories to remote failed to upload " + numberFailedAddCategories + " categories \r\n");
                            })
                        );

                        //SAVE NEW TRANSACTIONS FROM WEB TO DB
                        var numberAdded = 0;
                        var numberFailedAdd = 0;
                        foreach (var remoteCategory in remoteCategories)  //localTransactionsToPushUp
                        {
                            string stripped = remoteCategory.ToString();
                            stripped = stripped.Replace("{{", "{").Replace("}}", "}");
                            var t = JsonConvert.DeserializeObject<Repo.Entities.Category>(stripped);
                            var matchingCategory = localCategories.SingleOrDefault(x => x.Id == t.Id);
                            if (matchingCategory == null)
                            {
                                var newCategory = new Repo.Entities.Category
                                {
                                    Id = t.Id,
                                    Name = t.Name,
                                    SearchString = t.SearchString
                                };
                                context.Categories.Add(newCategory);
                                context.SaveChanges();
                            }
                        }
                        status.BeginInvoke(
                            new Action(() =>
                            {
                                status.AppendText("\r\ndb successfully added " + numberAdded + " categories to local db failed to add " + numberFailedAdd + " categories \r\n");
                                status.AppendText("\r\ncompleted local Categories \r\n");
                            })
                        );





                        //GET TRANSACTIONS
                        client = new RestClient("https://www.talkisbetter.com/api/banks");
                        request = new RestRequest(Method.GET);
                        request.AddHeader("jwt", jwt);
                        request.AddHeader("userId", userId);
                        request.AddHeader("content-type", "application/json");
                        response = client.Execute(request);
                        var remoteTransactions = JsonConvert.DeserializeObject<dynamic[]>(response.Content);
                        status.BeginInvoke(
                            new Action(() =>
                            {
                                status.AppendText("\r\nsuccessfully downloaded " + remoteTransactions.Length + " transactions\r\n");
                            })
                        );

                        successfullyDeleted = 0;
                        failedDelete = 0;

                        if (deleteExistingTransactions)
                        {
                            foreach (var remoteTransaction in remoteTransactions)  
                            {
                                client = new RestClient("https://www.talkisbetter.com/api/banks/"+ remoteTransaction._id);
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
                                    status.AppendText("\r\nsuccessfully deleted " + successfullyDeleted + " localTransactions failed to delete " + failedDelete + " localTransactions\r\n");
                                })
                            );
                        }

                        





                        //POST EACH NEW TRANSACTION
                        numberAdded = 0;
                        numberFailedAdd = 0;
                        client = new RestClient("https://www.talkisbetter.com/api/bank");
                        foreach (var localTransaction in localTransactions)  //localTransactionsToPushUp
                        {
                            var matchingTransaction = remoteTransactions.SingleOrDefault(x => x.SHA256 == localTransaction.SHA256);
                            if (deleteExistingTransactions || matchingTransaction == null)
                            {
                                localTransaction.userId = userId;
                                JsonSerializer serializer = new JsonSerializer();
                                serializer.Converters.Add(new JavaScriptDateTimeConverter());
                                serializer.NullValueHandling = NullValueHandling.Ignore;

                                string jsonTransaction = JsonConvert.SerializeObject(localTransaction, Formatting.Indented);
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
                                } //end of response code
                            } else
                            {
                                //todo merge the records somehow.
                            }
                        }
                        status.BeginInvoke(
                            new Action(() =>
                            {
                                status.AppendText("\r\nsuccessfully uploaded " + numberAdded + " transactions to remote failed to upload " + numberFailedAdd + " transactions \r\n");
                            })
                        );

                        //SAVE NEW TRANSACTIONS FROM WEB TO DB
                        numberAdded = 0;
                        numberFailedAdd = 0;
                        foreach (var remoteTransaction in remoteTransactions)  //localTransactionsToPushUp
                        {
                            string stripped = remoteTransaction.ToString();
                            stripped = stripped.Replace("{{", "{").Replace("}}", "}");
                            var t = JsonConvert.DeserializeObject<TransactionModel>(stripped);
                            var matchingTransaction = localTransactions.SingleOrDefault(x => x.SHA256 == t.SHA256);
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
                                status.AppendText("\r\ndb successfully added " + numberAdded + " transactions to local db failed to add " + numberFailedAdd + " transactions \r\n");
                                status.AppendText("\r\ncompleted localTransactions \r\n");
                            })
                        );




                    }
            ));
            backgroundThread.Start();


        }
    }
}
