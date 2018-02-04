using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests.ServiceTests;
using PersonalSpendingAnalysis.Models;

namespace UnitTests.ServiceTests
{
    [TestClass]
    public class TransactionServiceTest : ServiceTestBaseClass
    {
        //List<TransactionModel> GetTransactions();
        //List<TransactionModel> GetTransactions(global::Enums.orderBy currentOrder);
        //DateTime GetEarliestTransactionDate();
        //void UpdateTransactionCategory(Guid id, Guid? categoryId, string subCategory, bool manuallySet = false);
        //void AddNewTransaction(TransactionModel remoteTransaction);
        //TransactionModel GetTransaction(Guid transactionId);

        [TestMethod]
        public void TestListAddRemoveUpdate()
        {
            var startDate = new DateTime(2000,01,01);
            var endDate = DateTime.UtcNow;
            //check there is nothing in the repo
            var testArray = transactionService.GetTransactions(startDate,endDate).ToArray();
            Assert.AreEqual(testArray.Length, 0);

            //note by default the fake repo is used
            var transactionB = new TransactionModel
            {
                Id = Guid.NewGuid(),
                AccountId = Guid.NewGuid(),
                amount = 100,
                CategoryId = Guid.NewGuid(),
                ManualCategory = false,
                Notes = "notesB",
                SHA256 = "shaB",
                SubCategory = "subcatB",
                transactionDate = DateTime.UtcNow,
                userId = "userIdB"
            };
            var transactionA = new TransactionModel
            {
                Id = Guid.NewGuid(),
                AccountId = Guid.NewGuid(),
                amount = 100,
                CategoryId = Guid.NewGuid(),
                ManualCategory = false,
                Notes = "notesA",
                SHA256 = "shaA",
                SubCategory = "subcatA",
                transactionDate = DateTime.UtcNow.AddYears(-1),
                userId = "userIdA"
            };
            transactionService.AddNewTransaction(transactionA);
            transactionService.AddNewTransaction(transactionB);

            //test GetTransactions
            testArray = transactionService.GetTransactions(new DateTime(2000,01,01),DateTime.UtcNow).ToArray();
            //test that the results are NOT returned in name order
            Assert.AreEqual(testArray[0].Id, transactionA.Id);
            Assert.AreEqual(testArray[0].AccountId, transactionA.AccountId);
            Assert.AreEqual(testArray[0].amount, transactionA.amount);
            Assert.AreEqual(testArray[0].CategoryId, transactionA.CategoryId);
            Assert.AreEqual(testArray[0].ManualCategory, transactionA.ManualCategory);
            Assert.AreEqual(testArray[0].Notes, transactionA.Notes);
            Assert.AreEqual(testArray[0].SHA256, transactionA.SHA256);
            Assert.AreEqual(testArray[0].SubCategory, transactionA.SubCategory);
            Assert.AreEqual(testArray[0].transactionDate, transactionA.transactionDate);
            //userId should not be persisted in the repo
            Assert.AreNotEqual(testArray[0].userId, transactionA.userId);

            Assert.AreEqual(testArray[1].Id, transactionB.Id);
            Assert.AreEqual(testArray[1].AccountId, transactionB.AccountId);
            Assert.AreEqual(testArray[1].amount, transactionB.amount);
            Assert.AreEqual(testArray[1].CategoryId, transactionB.CategoryId);
            Assert.AreEqual(testArray[1].ManualCategory, transactionB.ManualCategory);
            Assert.AreEqual(testArray[1].Notes, transactionB.Notes);
            Assert.AreEqual(testArray[1].SHA256, transactionB.SHA256);
            Assert.AreEqual(testArray[1].SubCategory, transactionB.SubCategory);
            Assert.AreEqual(testArray[1].transactionDate, transactionB.transactionDate);
            //userId should not be persisted in the repo
            Assert.AreNotEqual(testArray[1].userId, transactionB.userId);
            //check that there are the correct number of records
            Assert.AreEqual(2, testArray.Length);


            testArray = transactionService.GetTransactions(Enums.orderBy.transactionDateDescending).ToArray();
            Assert.AreEqual(testArray[1].Id, transactionA.Id);
            Assert.AreEqual(testArray[1].AccountId, transactionA.AccountId);
            Assert.AreEqual(testArray[1].amount, transactionA.amount);
            Assert.AreEqual(testArray[1].CategoryId, transactionA.CategoryId);
            Assert.AreEqual(testArray[1].ManualCategory, transactionA.ManualCategory);
            Assert.AreEqual(testArray[1].Notes, transactionA.Notes);
            Assert.AreEqual(testArray[1].SHA256, transactionA.SHA256);
            Assert.AreEqual(testArray[1].SubCategory, transactionA.SubCategory);
            Assert.AreEqual(testArray[1].transactionDate, transactionA.transactionDate);
            //userId should not be persisted in the repo
            Assert.AreNotEqual(testArray[1].userId, transactionA.userId);

            Assert.AreEqual(testArray[0].Id, transactionB.Id);
            Assert.AreEqual(testArray[0].AccountId, transactionB.AccountId);
            Assert.AreEqual(testArray[0].amount, transactionB.amount);
            Assert.AreEqual(testArray[0].CategoryId, transactionB.CategoryId);
            Assert.AreEqual(testArray[0].ManualCategory, transactionB.ManualCategory);
            Assert.AreEqual(testArray[0].Notes, transactionB.Notes);
            Assert.AreEqual(testArray[0].SHA256, transactionB.SHA256);
            Assert.AreEqual(testArray[0].SubCategory, transactionB.SubCategory);
            Assert.AreEqual(testArray[0].transactionDate, transactionB.transactionDate);
            //userId should not be persisted in the repo
            Assert.AreNotEqual(testArray[0].userId, transactionB.userId);

            var start = transactionService.GetEarliestTransactionDate();
            Assert.AreEqual(start, transactionA.transactionDate);

            var newCategoryId = Guid.NewGuid();
            var newCat = new CategoryModel
            {
                Id = newCategoryId,
                Name = "testNewCategory",
                SearchString = "testSearchString"
            };
            categoryService.AddNewCategory(newCat);

            var newSubCat = "modded";
            transactionService.UpdateTransactionCategory(transactionB.Id, newCategoryId, newSubCat, true);
            var updatedTransaction = transactionService.GetTransaction(transactionB.Id);
            Assert.AreEqual(updatedTransaction.Id, transactionB.Id);
            Assert.AreEqual(updatedTransaction.AccountId, transactionB.AccountId);
            Assert.AreEqual(updatedTransaction.amount, transactionB.amount);
            Assert.AreEqual(updatedTransaction.CategoryId, newCategoryId);
            Assert.AreEqual(updatedTransaction.ManualCategory, true);
            Assert.AreEqual(updatedTransaction.Notes, transactionB.Notes);
            Assert.AreEqual(updatedTransaction.SHA256, transactionB.SHA256);
            Assert.AreEqual(updatedTransaction.SubCategory, newSubCat);
            Assert.AreEqual(updatedTransaction.transactionDate, transactionB.transactionDate);
            //userId should not be persisted in the repo
            Assert.AreNotEqual(updatedTransaction.userId, transactionB.userId);

            //category service injects separate instance of fakeRepo and therefore these are not persisted
            //Assert.AreEqual(updatedTransaction.Category.Id, newCat.Id);
            //Assert.AreEqual(updatedTransaction.Category.Name, newCat.Name);
            //Assert.AreEqual(updatedTransaction.Category.SearchString, newCat.SearchString);
            
        }

    }
}
