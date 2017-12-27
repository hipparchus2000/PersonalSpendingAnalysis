using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Moq;
using FluentAssertions;
using PersonalSpendingAnalysis.Dtos;
using PersonalSpendingAnalysis.Services;
using IRepositories.Interfaces;
using PersonalSpendingAnalysis.Models;

namespace ServiceUnitTestsUsingMoq
{
    [TestClass]
    public class BudgetsServiceMoqTest
    {
        [TestMethod]
        public void GetBudgets()
        {
            //Setup
            var budgetB = new BudgetDto
            {
                Amount = 100,
                CategoryName = "categoryB"
            };
            var budgetA = new BudgetDto
            {
                Amount = 200,
                CategoryName = "categoryA"
            };
            var list = new List<BudgetDto>();
            list.Add(budgetB);
            list.Add(budgetA);
            var repo = new Mock<IPersonalSpendingAnalysisRepo>();
            repo.Setup(x => x.GetBudgets()).Returns(list);

            var service = new BudgetsService(repo.Object);
            service.Should().NotBeNull();

            //test
            var result = service.GetBudgets().ToArray();

            //assert
            result.Should().NotBeNull();
            result[0].Amount.Should().Be(budgetB.Amount);
            result[1].Amount.Should().Be(budgetA.Amount);
            result[0].CategoryName.Should().Be(budgetB.CategoryName);
            result[1].CategoryName.Should().Be(budgetA.CategoryName);
            result.Length.Should().Be(2);
        }
        [TestMethod]
        public void CreateOrUpdateBudget()
        {
            //Setup
            var repo = new Mock<IPersonalSpendingAnalysisRepo>();

            var budgetModelB = new BudgetModel
            {
                Amount = 100,
                CategoryName = "categoryB"
            };
            var budgetModelA = new BudgetModel
            {
                Amount = 200,
                CategoryName = "categoryA"
            };
            var modelList = new List<BudgetModel>();
            modelList.Add(budgetModelB);
            modelList.Add(budgetModelA);

            var service = new BudgetsService(repo.Object);
            service.Should().NotBeNull();

            //test
            service.CreateOrUpdateBudgets(modelList);

            //assert
            repo.Verify(x => x.CreateOrUpdateBudgets(It.IsAny<List<BudgetDto>>()), Times.Once);
            repo.Verify(x => x.CreateOrUpdateBudgets(It.Is<List<BudgetDto>>(l=>l.Count==2)), Times.Once);
            repo.Verify(x => x.CreateOrUpdateBudgets(It.Is<List<BudgetDto>>(l => l[0].Amount == 100m)), Times.Once);
            repo.Verify(x => x.CreateOrUpdateBudgets(It.Is<List<BudgetDto>>(l => l[0].CategoryName.Equals("categoryB"))), Times.Once);
            repo.Verify(x => x.CreateOrUpdateBudgets(It.Is<List<BudgetDto>>(l => l[1].Amount == 200m)), Times.Once);
            repo.Verify(x => x.CreateOrUpdateBudgets(It.Is<List<BudgetDto>>(l => l[1].CategoryName.Equals("categoryA"))), Times.Once);
        }
    }
}
