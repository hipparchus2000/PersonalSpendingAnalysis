using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests.ServiceTests;
using PersonalSpendingAnalysis.Models;
using System.Collections.Generic;

namespace UnitTests
{
    [TestClass]
    public class BudgetsServiceTest : ServiceTestBaseClass
    {
        //List<BudgetModel> GetBudgets();
        //void CreateOrUpdateBudgets(List<BudgetModel> listOfBudgets);

        [TestMethod]
        public void CreateOrUpdateBudgetsAndListTestMethod1()
        {
            //check there is nothing in the repo
            var testArray = budgetsService.GetBudgets().ToArray();
            Assert.AreEqual(testArray.Length, 0);

            //note by default the fake repo is used
            var budgetB = new BudgetModel
            {
                Amount = 100,
                CategoryName = "categoryB"
            };
            var budgetA = new BudgetModel
            {
                Amount = 200,
                CategoryName = "categoryA"
            };
            var budgets = new List<BudgetModel>();
            budgets.Add(budgetB);
            budgets.Add(budgetA);
            budgetsService.CreateOrUpdateBudgets(budgets);
            testArray = budgetsService.GetBudgets().ToArray();
            //test that the results are returned in name order
            Assert.AreEqual(testArray[0].CategoryName, budgetA.CategoryName);
            Assert.AreEqual(testArray[0].Amount, budgetA.Amount);
            Assert.AreEqual(testArray[1].CategoryName, budgetB.CategoryName);
            Assert.AreEqual(testArray[1].Amount, budgetB.Amount);
            //check that 
            Assert.AreEqual(budgets.Count, testArray.Length);

            //this should update categoryB to 3000 from 200
            budgetB.Amount = 300;
            budgetB.CategoryName = "categoryB";
           
            var budgetC = new BudgetModel
            {
                Amount = 400,
                CategoryName = "categoryC"
            };
            budgets = new List<BudgetModel>();
            budgets.Add(budgetC);
            budgets.Add(budgetB);

            budgetsService.CreateOrUpdateBudgets(budgets);
            testArray = budgetsService.GetBudgets().ToArray();
            //test that the results are returned in name order
            Assert.AreEqual(testArray[0].CategoryName, budgetA.CategoryName);
            Assert.AreEqual(testArray[0].Amount, budgetA.Amount);
            Assert.AreEqual(testArray[1].CategoryName, budgetB.CategoryName);
            Assert.AreEqual(testArray[1].Amount, budgetB.Amount);
            Assert.AreEqual(testArray[2].CategoryName, budgetC.CategoryName);
            Assert.AreEqual(testArray[2].Amount, budgetC.Amount);
            //check that there are now 3 records
            Assert.AreEqual(testArray.Length, 3);

        }
    }
}
