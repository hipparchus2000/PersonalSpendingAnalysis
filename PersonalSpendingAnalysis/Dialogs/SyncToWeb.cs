using IServices.Interfaces;
using PersonalSpendingAnalysis.IServices;
using PersonalSpendingAnalysis.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace PersonalSpendingAnalysis.Dialogs
{
    public partial class SyncToWeb : Form
    {
        IImportsAndExportService importsAndExportService;
        ITransactionService transactionsService;
        ICategoryService categoryService;

        public SyncToWeb(IImportsAndExportService _importsAndExportService, ITransactionService _transactionsService, ICategoryService _categoryService)
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
            importsAndExportService = _importsAndExportService;
            transactionsService = _transactionsService;
            categoryService = _categoryService;
        }

        private void buttonSyncToWeb_Click(object sender, EventArgs e)
        {
            status.Text = "";
            var username = this.UsernameTextBox.Text;
            var password = this.passwordTextBox.Text;
            storeUserNameAndPasswordInRegistry(username, password);
            var deleteExistingTransactions = deleteExistingTransactionsCheckbox.Checked;

            var loginResult = importsAndExportService.LoginToWebService(username, password);
            if ( loginResult.success == false)
            {
                status.AppendText(username + "failed to log in\r\n");
                return;
            }
            status.AppendText("userId: " + loginResult.userId + "\r\n");
            
            Thread backgroundThread = new Thread(
                    new ThreadStart(() =>
                    {
                        //categories first to make sure primary keys are ok
                        var categoryResponse = importsAndExportService.GetRemoteCategories(loginResult);
                        if (categoryResponse.Success == false)
                        {
                            status.BeginInvoke(
                            new Action(() =>
                                {
                                    status.AppendText("\r\nError encountered reading categories " + categoryResponse.ErrorMessage + " \r\n");
                                })
                            );
                            return;
                        }

                        status.BeginInvoke(
                            new Action(() =>
                            {
                                status.AppendText("\r\nsuccessfully downloaded " + categoryResponse.remoteCategories.Count + " categories\r\n");
                            })
                        );

                        var successfullyDeleted = 0;
                        var failedDelete = 0;

                        if (deleteExistingTransactions)
                        {
                            foreach (var remoteCategory in categoryResponse.remoteCategories)
                            {
                                var deleteResponseSuccess = importsAndExportService.DeleteRemoteCategory(loginResult, remoteCategory._id);
                                if (deleteResponseSuccess)
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

                        var localCategories = categoryService.GetCategories().Select(x => new {
                            Id = x.Id,
                            Name = x.Name,
                            SearchString = x.SearchString,
                            userId = loginResult.userId
                        }).ToList();
                        var localTransactions = transactionsService.GetTransactions().Select(x => new TransactionModel
                        {
                            Id = x.Id,
                            amount = x.amount,
                            transactionDate = x.transactionDate,
                            Notes = x.Notes,
                            CategoryId = x.CategoryId,
                            SubCategory = x.SubCategory,
                            AccountId = x.AccountId,
                            SHA256 = x.SHA256,
                            userId = loginResult.userId,
                            ManualCategory = x.ManualCategory
                        }).ToList();


                        //POST EACH NEW CATEGORY
                        var numberOfCategoriesAdded = 0;
                        var numberFailedAddCategories = 0;
                        foreach (var localCategory in localCategories)
                        {
                            var matchingCategory = categoryResponse.remoteCategories.SingleOrDefault(x => x.Id == localCategory.Id);
                            if (deleteExistingTransactions || matchingCategory == null)
                            {
                                //no remote category for the localCategory
                                var postResponse = importsAndExportService.PostNewCategoryToRemote(loginResult,localCategory);

                                if (postResponse)
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

                        //SAVE NEW CATEGORIES FROM WEB TO DB
                        var numberAdded = 0;
                        var numberFailedAdd = 0;
                        foreach (var remoteCategory in categoryResponse.remoteCategories)  
                        {
                            string stripped = remoteCategory.ToString();
                            stripped = stripped.Replace("{{", "{").Replace("}}", "}");
                            var t = importsAndExportService.ExtractCategoryModelFromJson(stripped);
                            var matchingCategory = localCategories.SingleOrDefault(x => x.Id == t.Id);
                            if (matchingCategory == null)
                            {
                                categoryService.AddNewCategory(t);
                            }
                        }
                        status.BeginInvoke(
                            new Action(() =>
                            {
                                status.AppendText("\r\ndb successfully added " + numberAdded + " categories to local db failed to add " + numberFailedAdd + " categories \r\n");
                                status.AppendText("\r\ncompleted local Categories \r\n");
                            })
                        );





                        //TRANSACTIONS
                        var remoteTransactions = importsAndExportService.GetRemoteTransactions(loginResult);
                        if (remoteTransactions.Success)
                        {
                            status.BeginInvoke(
                                new Action(() =>
                                {
                                    status.AppendText("\r\nsuccessfully downloaded " + remoteTransactions.remoteTransactions.Count + " transactions\r\n");
                                })
                            );
                        }
                        else
                        {
                            status.BeginInvoke(
                                new Action(() =>
                                {
                                    status.AppendText("\r\failed to download transactions " + remoteTransactions.ErrorMessage + " \r\n");
                                })
                            );
                            return;
                        }

                        successfullyDeleted = 0;
                        failedDelete = 0;

                        if (deleteExistingTransactions)
                        {
                            foreach (var remoteTransaction in remoteTransactions.remoteTransactions)  
                            {
                                var success = importsAndExportService.DeleteRemoteTransaction(loginResult, remoteTransaction._id);
                                if(success)
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

                        foreach (var localTransaction in localTransactions)  //localTransactionsToPushUp
                        {
                            var matchingTransaction = remoteTransactions.remoteTransactions.SingleOrDefault(x => x.SHA256 == localTransaction.SHA256);
                            if (deleteExistingTransactions || matchingTransaction == null)
                            {
                                var success = importsAndExportService.PostNewTransactionToRemote(loginResult, localTransaction);
                                if (success)
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
                        foreach (var remoteTransaction in remoteTransactions.remoteTransactions)  //localTransactionsToPushUp
                        {
                            string stripped = remoteTransaction.ToString();
                            stripped = stripped.Replace("{{", "{").Replace("}}", "}");
                            var t = importsAndExportService.ExtractTransactionModelFromJson(stripped);
                            var matchingTransaction = localTransactions.SingleOrDefault(x => x.SHA256 == t.SHA256);
                            if (matchingTransaction == null)
                            {
                                transactionsService.AddNewTransaction(remoteTransaction);
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

        private void storeUserNameAndPasswordInRegistry(string username, string password)
        {
            var userRegistryKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("PSAauth");
            userRegistryKey.SetValue("User", username);
            userRegistryKey.SetValue("Pwd", password);
            userRegistryKey.Close();
        }
    }
}
