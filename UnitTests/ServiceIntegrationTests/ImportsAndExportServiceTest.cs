using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests.ServiceTests;
using System.Reflection;
using System.IO;
using PersonalSpendingAnalysis.Models;

namespace UnitTests.ServiceTests
{
    [TestClass]
    public class ImportsAndExportServiceTest : ServiceTestBaseClass
    {
        //these bits are immediately testable
        //ImportResults ImportCsv(string csvText);  --DONE
        //string GetExportableText(); --DONE
        //ImportResult ImportJson(string fileText);  --DONE

            //these are two web helper methods:
        //CategoryModel ExtractCategoryModelFromJson(string stripped);
        //TransactionModel ExtractTransactionModelFromJson(string stripped);

        //todo figure out how to do remote stuff -- perhaps moq it?
        //LoginResult LoginToWebService(string username, string password);

        //RemoteCategoryModel GetRemoteCategories(LoginResult loginResult);
        //bool PostNewCategoryToRemote(LoginResult loginResult, object localCategory);
        //bool DeleteRemoteCategory(LoginResult loginResult, Guid id);

        //RemoteTransactionModel GetRemoteTransactions(LoginResult loginResult);
        //bool PostNewTransactionToRemote(LoginResult loginResult, object localCategory);
        //bool DeleteRemoteTransaction(LoginResult loginResult, Guid id);

        [TestMethod]
        public void TestImportCsv()
        {
            
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "UnitTests.testCsvs.testCsv1.csv";

            Stream stream = assembly.GetManifestResourceStream(resourceName);
            StreamReader reader = new StreamReader(stream);
            string result = reader.ReadToEnd();
            var results = importsAndExportService.ImportCsv(result);
            var transactionArray = transactionService.GetTransactions(Enums.orderBy.transactionDateDescending);

            Assert.AreEqual(transactionArray[0].transactionDate, DateTime.Parse("09/12/2017"));
            Assert.AreEqual(transactionArray[1].transactionDate, DateTime.Parse("08/12/2017"));
            Assert.AreEqual(transactionArray[2].transactionDate, DateTime.Parse("07/12/2017"));
            Assert.AreEqual(transactionArray[3].transactionDate, DateTime.Parse("06/12/2017"));

            Assert.AreEqual(transactionArray[0].amount, 200.00m);
            Assert.AreEqual(transactionArray[1].amount, -10.00m);
            Assert.AreEqual(transactionArray[2].amount, -20.00m);
            Assert.AreEqual(transactionArray[3].amount, -30.00m);

            Assert.AreEqual(transactionArray[0].Notes, "'Pay 1");
            Assert.AreEqual(transactionArray[1].Notes, "'Bill 1");
            Assert.AreEqual(transactionArray[2].Notes, "'XYZ 07DEC17 C  PLACE NAME2  ADDRESS2");
            Assert.AreEqual(transactionArray[3].Notes, "'XYZ 06DEC17 C  PLACE NAME2  ADDRESS2");

            Assert.AreEqual(results.NumberOfDuplicatesFound, 0);
            Assert.AreEqual(results.NumberOfRecordsImported, 4);



            //test that duplicates are not made - i.e. identical rows are ignored
            resourceName = "UnitTests.testCsvs.testCsv2.csv";

            stream = assembly.GetManifestResourceStream(resourceName);
            reader = new StreamReader(stream);
            result = reader.ReadToEnd();
            results = importsAndExportService.ImportCsv(result);
            transactionArray = transactionService.GetTransactions(Enums.orderBy.transactionDateDescending);

            Assert.AreEqual(transactionArray[0].transactionDate, DateTime.Parse("09/12/2017"));
            Assert.AreEqual(transactionArray[1].transactionDate, DateTime.Parse("08/12/2017"));
            Assert.AreEqual(transactionArray[2].transactionDate, DateTime.Parse("07/12/2017"));
            Assert.AreEqual(transactionArray[3].transactionDate, DateTime.Parse("06/12/2017"));
            Assert.AreEqual(transactionArray[4].transactionDate, DateTime.Parse("05/12/2017"));

            Assert.AreEqual(transactionArray[0].amount, 200.00m);
            Assert.AreEqual(transactionArray[1].amount, -10.00m);
            Assert.AreEqual(transactionArray[2].amount, -20.00m);
            Assert.AreEqual(transactionArray[3].amount, -30.00m);
            Assert.AreEqual(transactionArray[4].amount, -40.00m);

            Assert.AreEqual(transactionArray[0].Notes, "'Pay 1");
            Assert.AreEqual(transactionArray[1].Notes, "'Bill 1");
            Assert.AreEqual(transactionArray[2].Notes, "'XYZ 07DEC17 C  PLACE NAME2  ADDRESS2");
            Assert.AreEqual(transactionArray[3].Notes, "'XYZ 06DEC17 C  PLACE NAME2  ADDRESS2");
            Assert.AreEqual(transactionArray[4].Notes, "'XYZ 06DEC17 C  PLACE NAME3  ADDRESS3");

            Assert.AreEqual(results.NumberOfDuplicatesFound, 1);
            Assert.AreEqual(results.NumberOfRecordsImported, 2);
            Assert.AreEqual(transactionArray.Count, 5);
            
        }

        [TestMethod]
        public void TestExportJson()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "UnitTests.testCsvs.testCsv1.csv";

            Stream stream = assembly.GetManifestResourceStream(resourceName);
            StreamReader reader = new StreamReader(stream);
            string result = reader.ReadToEnd();
            var results = importsAndExportService.ImportCsv(result);
            var transactionArray = transactionService.GetTransactions(Enums.orderBy.transactionDateDescending);

            Assert.AreEqual(transactionArray[0].transactionDate, DateTime.Parse("09/12/2017"));
            Assert.AreEqual(transactionArray[1].transactionDate, DateTime.Parse("08/12/2017"));
            Assert.AreEqual(transactionArray[2].transactionDate, DateTime.Parse("07/12/2017"));
            Assert.AreEqual(transactionArray[3].transactionDate, DateTime.Parse("06/12/2017"));

            Assert.AreEqual(transactionArray[0].amount, 200.00m);
            Assert.AreEqual(transactionArray[1].amount, -10.00m);
            Assert.AreEqual(transactionArray[2].amount, -20.00m);
            Assert.AreEqual(transactionArray[3].amount, -30.00m);

            Assert.AreEqual(transactionArray[0].Notes, "'Pay 1");
            Assert.AreEqual(transactionArray[1].Notes, "'Bill 1");
            Assert.AreEqual(transactionArray[2].Notes, "'XYZ 07DEC17 C  PLACE NAME2  ADDRESS2");
            Assert.AreEqual(transactionArray[3].Notes, "'XYZ 06DEC17 C  PLACE NAME2  ADDRESS2");

            Assert.AreEqual(results.NumberOfDuplicatesFound, 0);
            Assert.AreEqual(results.NumberOfRecordsImported, 4);

            transactionArray = transactionService.GetTransactions(Enums.orderBy.transactionDateDescending);

            var categoryB = new CategoryModel
            {
                Id = Guid.NewGuid(),
                Name = "categoryB",
                SearchString = "catB,catB1"
            };
            var categoryA = new CategoryModel
            {
                Id = Guid.NewGuid(),
                Name = "categoryA",
                SearchString = "catA,catA1"
            };
            categoryService.AddNewCategory(categoryA);
            categoryService.AddNewCategory(categoryB);
            transactionService.UpdateTransactionCategory(transactionArray[0].Id, categoryB.Id, "testSubCatB", false);
            transactionService.UpdateTransactionCategory(transactionArray[1].Id, categoryA.Id, "testSubCatA", true);


            var json = importsAndExportService.GetExportableText();
            personalSpendingAnalysisRepo.ClearFakeRepo();
            importsAndExportService.ImportJson(json);

            transactionArray = transactionService.GetTransactions(Enums.orderBy.transactionDateDescending);

            Assert.AreEqual(transactionArray[0].transactionDate, DateTime.Parse("09/12/2017"));
            Assert.AreEqual(transactionArray[1].transactionDate, DateTime.Parse("08/12/2017"));
            Assert.AreEqual(transactionArray[2].transactionDate, DateTime.Parse("07/12/2017"));
            Assert.AreEqual(transactionArray[3].transactionDate, DateTime.Parse("06/12/2017"));

            Assert.AreEqual(transactionArray[0].amount, 200.00m);
            Assert.AreEqual(transactionArray[1].amount, -10.00m);
            Assert.AreEqual(transactionArray[2].amount, -20.00m);
            Assert.AreEqual(transactionArray[3].amount, -30.00m);

            Assert.AreEqual(transactionArray[0].Notes, "'Pay 1");
            Assert.AreEqual(transactionArray[1].Notes, "'Bill 1");
            Assert.AreEqual(transactionArray[2].Notes, "'XYZ 07DEC17 C  PLACE NAME2  ADDRESS2");
            Assert.AreEqual(transactionArray[3].Notes, "'XYZ 06DEC17 C  PLACE NAME2  ADDRESS2");

            Assert.AreEqual(transactionArray[0].CategoryId, categoryB.Id);
            Assert.AreEqual(transactionArray[1].CategoryId, categoryA.Id);
            Assert.AreEqual(transactionArray[2].CategoryId, null);
            Assert.AreEqual(transactionArray[3].CategoryId, null);

            Assert.AreEqual(transactionArray[0].SubCategory, "testSubCatB");
            Assert.AreEqual(transactionArray[1].SubCategory, "testSubCatA");
            Assert.AreEqual(transactionArray[2].SubCategory, null);
            Assert.AreEqual(transactionArray[3].SubCategory, null);

            Assert.AreEqual(transactionArray[0].ManualCategory, false);
            Assert.AreEqual(transactionArray[1].ManualCategory, true);
            Assert.AreEqual(transactionArray[2].ManualCategory, false);
            Assert.AreEqual(transactionArray[3].ManualCategory, false);

            Assert.AreEqual(transactionArray.Count, 4);

            var categories = categoryService.GetCategories().ToArray();

            Assert.AreEqual(categories[0].Id, categoryA.Id);
            Assert.AreEqual(categories[1].Id, categoryB.Id);
            Assert.AreEqual(categories[0].Name, categoryA.Name);
            Assert.AreEqual(categories[1].Name, categoryB.Name);
            Assert.AreEqual(categories[0].SearchString, categoryA.SearchString);
            Assert.AreEqual(categories[1].SearchString, categoryB.SearchString);

            Assert.AreEqual(categories.Length, 2);
            

        }
    }
}
