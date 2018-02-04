using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using IRepositories.Interfaces;
using PersonalSpendingAnalysis.Models;
using Services.Services;
using FluentAssertions;
using PersonalSpendingAnalysis.Dtos;
using System.Collections.Generic;
using System.Linq;

namespace ServiceUnitTestsUsingMoq
{
    [TestClass]
    public class TransactionServiceMoqTest
    {
        [TestMethod]
        public void GetTransactions()
        {
            //Setup
            var categoryB = new CategoryDto
            {
                Name = "categoryB",
                SearchString = "catBsearchString",
                Id = Guid.NewGuid()
            };
            var categoryA = new CategoryDto
            {
                Name = "categoryA",
                SearchString = "catAsearchString",
                Id = Guid.NewGuid()
            };

            var transactionA = new TransactionDto
            {
                Id = Guid.NewGuid(),
                AccountId = null,
                amount = 100.0m,
                CategoryId = categoryB.Id,
                Category = categoryB,
                ManualCategory = false,
                Notes = "notes",
                SubCategory = "subCategory",
                transactionDate = DateTime.UtcNow,
                SHA256 = "sha256"
            };

            var transactionB = new TransactionDto
            {
                Id = Guid.NewGuid(),
                AccountId = null,
                amount = 100.0m,
                CategoryId = categoryA.Id,
                Category = categoryA,
                ManualCategory = false,
                Notes = "notes",
                SubCategory = "subCategory",
                transactionDate = DateTime.UtcNow,
                SHA256 = "sha256"
            };

            var list = new List<TransactionDto>();
            list.Add(transactionA);
            list.Add(transactionB);
            var repo = new Mock<IPersonalSpendingAnalysisRepo>();
            repo.Setup(x => x.GetTransactions(new DateTime(2000,01,01),DateTime.UtcNow)).Returns(list);

            var service = new TransactionService(repo.Object);
            service.Should().NotBeNull();

            //test
            var result = service.GetTransactions(new DateTime(2000, 01, 01), DateTime.UtcNow).ToArray();

            //assert
            result.Should().NotBeNull();
            result.Length.Should().Be(2);
            result[0].Id.Should().Be(transactionA.Id);
            result[0].AccountId.Should().Be(transactionA.AccountId);
            result[0].amount.Should().Be(transactionA.amount);
            result[0].CategoryId.Should().Be(transactionA.CategoryId);
            result[0].Category.Id.Should().Be(transactionA.Category.Id);
            result[0].Category.Name.Should().Be(transactionA.Category.Name);
            result[0].Category.SearchString.Should().Be(transactionA.Category.SearchString);
            result[0].ManualCategory.Should().Be(transactionA.ManualCategory);
            result[0].Notes.Should().Be(transactionA.Notes);
            result[0].transactionDate.Should().Be(transactionA.transactionDate);
            result[0].SHA256.Should().Be(transactionA.SHA256);

            result[1].Id.Should().Be(transactionB.Id);
            result[1].AccountId.Should().Be(transactionB.AccountId);
            result[1].amount.Should().Be(transactionB.amount);
            result[1].CategoryId.Should().Be(transactionB.CategoryId);
            result[1].Category.Id.Should().Be(transactionB.Category.Id);
            result[1].Category.Name.Should().Be(transactionB.Category.Name);
            result[1].Category.SearchString.Should().Be(transactionB.Category.SearchString);
            result[1].ManualCategory.Should().Be(transactionB.ManualCategory);
            result[1].Notes.Should().Be(transactionB.Notes);
            result[1].transactionDate.Should().Be(transactionB.transactionDate);
            result[1].SHA256.Should().Be(transactionB.SHA256);
        }

        [TestMethod]
        public void GetTransactions_orderedbyAmountAscending()
        {
            //Setup
            var categoryB = new CategoryDto
            {
                Name = "categoryB",
                SearchString = "catBsearchString",
                Id = Guid.NewGuid()
            };
            var categoryA = new CategoryDto
            {
                Name = "categoryA",
                SearchString = "catAsearchString",
                Id = Guid.NewGuid()
            };

            var transactionA = new TransactionDto
            {
                Id = Guid.NewGuid(),
                AccountId = null,
                amount = 200.0m,
                CategoryId = categoryB.Id,
                Category = categoryB,
                ManualCategory = false,
                Notes = "notes",
                SubCategory = "subCategory",
                transactionDate = DateTime.UtcNow,
                SHA256 = "sha256"
            };

            var transactionB = new TransactionDto
            {
                Id = Guid.NewGuid(),
                AccountId = null,
                amount = 100.0m,
                CategoryId = categoryA.Id,
                Category = categoryA,
                ManualCategory = false,
                Notes = "notes",
                SubCategory = "subCategory",
                transactionDate = DateTime.UtcNow,
                SHA256 = "sha256"
            };

            var list = new List<TransactionDto>();
            list.Add(transactionA);
            list.Add(transactionB);
            var repo = new Mock<IPersonalSpendingAnalysisRepo>();
            repo.Setup(x => x.GetTransactions(Enums.orderBy.amountAscending)).Returns(list);

            var service = new TransactionService(repo.Object);
            service.Should().NotBeNull();

            //test
            var result = service.GetTransactions(Enums.orderBy.amountAscending).ToArray();

            //assert
            result.Should().NotBeNull();
            result.Length.Should().Be(2);
            result[0].Id.Should().Be(transactionA.Id);
            result[0].AccountId.Should().Be(transactionA.AccountId);
            result[0].amount.Should().Be(transactionA.amount);
            result[0].CategoryId.Should().Be(transactionA.CategoryId);
            result[0].Category.Id.Should().Be(transactionA.Category.Id);
            result[0].Category.Name.Should().Be(transactionA.Category.Name);
            result[0].Category.SearchString.Should().Be(transactionA.Category.SearchString);
            result[0].ManualCategory.Should().Be(transactionA.ManualCategory);
            result[0].Notes.Should().Be(transactionA.Notes);
            result[0].transactionDate.Should().Be(transactionA.transactionDate);
            result[0].SHA256.Should().Be(transactionA.SHA256);

            result[1].Id.Should().Be(transactionB.Id);
            result[1].AccountId.Should().Be(transactionB.AccountId);
            result[1].amount.Should().Be(transactionB.amount);
            result[1].CategoryId.Should().Be(transactionB.CategoryId);
            result[1].Category.Id.Should().Be(transactionB.Category.Id);
            result[1].Category.Name.Should().Be(transactionB.Category.Name);
            result[1].Category.SearchString.Should().Be(transactionB.Category.SearchString);
            result[1].ManualCategory.Should().Be(transactionB.ManualCategory);
            result[1].Notes.Should().Be(transactionB.Notes);
            result[1].transactionDate.Should().Be(transactionB.transactionDate);
            result[1].SHA256.Should().Be(transactionB.SHA256);

        }

        [TestMethod]
        public void GetEarliestTransactionDate()
        {
            var repo = new Mock<IPersonalSpendingAnalysisRepo>();
            var dateTime = DateTime.UtcNow;
            repo.Setup(x => x.GetEarliestTransactionDate()).Returns(dateTime);

            var service = new TransactionService(repo.Object);
            service.Should().NotBeNull();

            //test
            var result = service.GetEarliestTransactionDate();

            //assert
            result.Should().Be(dateTime);
        }

        [TestMethod]
        public void UpdateTransactionCategory()
        {
            //Setup
            var categoryB = new CategoryModel
            {
                Name = "categoryB",
                SearchString = "catBsearchString",
                Id = Guid.NewGuid()
            };
            var transaction = new TransactionModel
            {
                Id = Guid.NewGuid(),
                AccountId = null,
                amount = 100.0m,
                CategoryId = categoryB.Id,
                Category = categoryB,
                ManualCategory = false,
                Notes = "notes",
                SubCategory = "subCategory",
                transactionDate = DateTime.UtcNow,
                userId = "userID",   //not persisted
                SHA256 = "sha256"
            };
            var repo = new Mock<IPersonalSpendingAnalysisRepo>();
            var service = new TransactionService(repo.Object);
            service.Should().NotBeNull();

            //Test
            service.UpdateTransactionCategory(transaction.Id,categoryB.Id,"newSubcat",true);

            //Assert
            repo.Verify(x => x.UpdateTransactionCategory(It.IsAny<Guid>(),It.IsAny<Guid?>(),It.IsAny<string>(),It.IsAny<bool>()), Times.Once);
            repo.Verify(x => x.UpdateTransactionCategory(
                It.Is<Guid>(l => l == transaction.Id),
                It.Is<Guid?>(l => l == categoryB.Id),
                It.Is<string>(l => l == "newSubcat"),
                It.Is<bool>(l => l == true)
                ), Times.Once);
            
        }

        [TestMethod]
        public void AddNewTransaction()
        {
            //Setup
            var categoryB = new CategoryModel
            {
                Name = "categoryB",
                SearchString = "catBsearchString",
                Id = Guid.NewGuid()
            };
            var transaction = new TransactionModel
            {
                Id = Guid.NewGuid(),
                AccountId = null,
                amount = 100.0m,
                CategoryId = categoryB.Id,
                Category = categoryB,
                ManualCategory = true,
                Notes = "notes",
                SubCategory = "subCategory",
                transactionDate = DateTime.UtcNow,
                userId = "userID",   //not persisted
                SHA256 = "sha256"
            };
            var repo = new Mock<IPersonalSpendingAnalysisRepo>();
            var service = new TransactionService(repo.Object);
            service.Should().NotBeNull();

            //Test
            service.AddNewTransaction(transaction);

            //Assert
            repo.Verify(x => x.AddTransaction(It.IsAny<TransactionDto>()), Times.Once);
            repo.Verify(x => x.AddTransaction(It.Is<TransactionDto>(l => l.Id == transaction.Id)), Times.Once);
            repo.Verify(x => x.AddTransaction(It.Is<TransactionDto>(l => l.AccountId == transaction.AccountId)), Times.Once);
            repo.Verify(x => x.AddTransaction(It.Is<TransactionDto>(l => l.amount == transaction.amount)), Times.Once);
            repo.Verify(x => x.AddTransaction(It.Is<TransactionDto>(l => l.CategoryId == transaction.CategoryId)), Times.Once);
            repo.Verify(x => x.AddTransaction(It.Is<TransactionDto>(l => l.Category == null)), Times.Once);
            repo.Verify(x => x.AddTransaction(It.Is<TransactionDto>(l => l.ManualCategory == transaction.ManualCategory)), Times.Once);
            repo.Verify(x => x.AddTransaction(It.Is<TransactionDto>(l => l.Notes == transaction.Notes)), Times.Once);
            repo.Verify(x => x.AddTransaction(It.Is<TransactionDto>(l => l.SubCategory == transaction.SubCategory)), Times.Once);
            repo.Verify(x => x.AddTransaction(It.Is<TransactionDto>(l => l.transactionDate == transaction.transactionDate)), Times.Once);
            repo.Verify(x => x.AddTransaction(It.Is<TransactionDto>(l => l.SHA256 == transaction.SHA256)), Times.Once);

        }
        [TestMethod]
        public void GetTransaction()
        {
            //Setup
            var categoryB = new CategoryDto
            {
                Name = "categoryB",
                SearchString = "catBsearchString",
                Id = Guid.NewGuid()
            };

            var transactionA = new TransactionDto
            {
                Id = Guid.NewGuid(),
                AccountId = null,
                amount = 100.0m,
                CategoryId = categoryB.Id,
                Category = categoryB,
                ManualCategory = false,
                Notes = "notes",
                SubCategory = "subCategory",
                transactionDate = DateTime.UtcNow,
                SHA256 = "sha256"
            };

            var repo = new Mock<IPersonalSpendingAnalysisRepo>();
            repo.Setup(x => x.GetTransaction(transactionA.Id)).Returns(transactionA);

            var service = new TransactionService(repo.Object);
            service.Should().NotBeNull();

            //test
            var result = service.GetTransaction(transactionA.Id);

            //assert
            result.Should().NotBeNull();
            result.Id.Should().Be(transactionA.Id);
            result.AccountId.Should().Be(transactionA.AccountId);
            result.amount.Should().Be(transactionA.amount);
            result.CategoryId.Should().Be(transactionA.CategoryId);
            result.Category.Id.Should().Be(transactionA.Category.Id);
            result.Category.Name.Should().Be(transactionA.Category.Name);
            result.Category.SearchString.Should().Be(transactionA.Category.SearchString);
            result.ManualCategory.Should().Be(transactionA.ManualCategory);
            result.Notes.Should().Be(transactionA.Notes);
            result.transactionDate.Should().Be(transactionA.transactionDate);
            result.SHA256.Should().Be(transactionA.SHA256);
            
        }

    }
}
