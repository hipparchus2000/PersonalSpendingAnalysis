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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PersonalSpendingAnalysis.Dialogs
{
    public partial class SyncToWeb : Form
    {
        public SyncToWeb()
        {
            InitializeComponent();
        }

        private void buttonSyncToWeb_Click(object sender, EventArgs e)
        {

            var username = this.UsernameTextBox.Text;
            var password = this.passwordTextBox.Text;

            var client = new RestClient("https://www.talkisbetter.com/api/auth");
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", "{\r\n    \"username\":\""+username+"\",\r\n    \"password\":\""+password+"\"\r\n}\r\n", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            dynamic loginResult = JsonConvert.DeserializeObject<dynamic>(response.Content);
            var jwt = loginResult.token.Value;
            var userId = (string)loginResult.userId.Value;
            status.AppendText(userId);

            client = new RestClient("https://www.talkisbetter.com/api/banks");
            request = new RestRequest(Method.GET);
            request.AddHeader("jwt", jwt);
            request.AddHeader("userId", userId);
            request.AddHeader("content-type", "application/json");
            response = client.Execute(request);
            dynamic myTransactions = JsonConvert.DeserializeObject<dynamic>(response.Content);

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

            var webTransactions = new List<Repo.Entities.Transaction>();
            //foreach(var transaction in myTransactions)
            //{
            //    var transactionEntity = new Repo.Entities.Transaction
            //    {
            //        Id = transaction.Id==null?null:transaction.Id.Value,
            //        amount = transaction.amount.Value,
            //        transactionDate = transaction.transactionDate.Value,
            //        Notes = transaction.Notes.Value,
            //        CategoryId = transaction.CategoryId.Value,
            //        SubCategory = transaction.SubCategory.Value,
            //        AccountId = transaction.AccountId.Value,
            //        SHA256 = transaction.SHA256.Value,
            //        ManualCategory = transaction.ManualCategory.Value
            //    };
            //    webTransactions.Add(transactionEntity);
            //}
            
            //var transactionsToPushUp = myTransactions.Except(webTransactions);
            foreach (var transaction in transactions)  //transactionsToPushUp
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
                request.AddParameter("application/json", jsonTransaction , ParameterType.RequestBody);
                response = client.Execute(request);
                
            }




        }
    }
}
