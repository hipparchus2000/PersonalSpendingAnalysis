using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests.ServiceTests;
using PersonalSpendingAnalysis.Models;
using System.Collections.Generic;

namespace UnitTests.ServiceTests
{
    [TestClass]
    public class CategoryServiceTest : ServiceTestBaseClass
    {
        //List<CategoryModel> GetCategories();
        //List<String> GetListOfCategories();
        //CategoryModel AddNewCategory(CategoryModel categoryModel);
        //void RemoveCategory(CategoryModel deletedCategory);
        //void UpdateCategorySearchString(Guid value, string text);
        [TestMethod]
        public void TestListAddRemoveUpdate()
        {            
            //check there is nothing in the repo
            var testArray = categoryService.GetCategories().ToArray();
            Assert.AreEqual(testArray.Length, 0);

            //note by default the fake repo is used
            var categoryB = new CategoryModel
            {
                Id = Guid.NewGuid(),
                Name = "categoryB",
                SearchString  ="catB,catB1"
            };
            var categoryA = new CategoryModel
            {
                Id = Guid.NewGuid(),
                Name = "categoryA",
                SearchString = "catA,catA1"
            };
            categoryService.AddNewCategory(categoryA);
            categoryService.AddNewCategory(categoryB);

            //test GetCategories
            testArray = categoryService.GetCategories().ToArray();
            //test that the results are NOT returned in name order
            Assert.AreEqual(testArray[0].Name, categoryA.Name);
            Assert.AreEqual(testArray[0].Id, categoryA.Id);
            Assert.AreEqual(testArray[0].SearchString, categoryA.SearchString);
            Assert.AreEqual(testArray[1].Name, categoryB.Name);
            Assert.AreEqual(testArray[1].Id, categoryB.Id);
            Assert.AreEqual(testArray[1].SearchString, categoryB.SearchString);
            //check that there are the correct number of records
            Assert.AreEqual(2, testArray.Length);

            //test GetListOfCategories
            var testStringArray = categoryService.GetListOfCategories().ToArray();
            //test that the results are returned in name order
            Assert.AreEqual(testStringArray[0], categoryA.Name);
            Assert.AreEqual(testStringArray[1], categoryB.Name);
            //check that there are the correct number of records
            Assert.AreEqual(2, testStringArray.Length);

            //void UpdateCategorySearchString(Guid value, string text);
            categoryService.UpdateCategorySearchString(categoryB.Id, "test");
            testArray = categoryService.GetCategories().ToArray();
            //test that the results are returned in name order
            Assert.AreEqual(testArray[0].Name, categoryA.Name);
            Assert.AreEqual(testArray[0].Id, categoryA.Id);
            Assert.AreEqual(testArray[0].SearchString, categoryA.SearchString);
            Assert.AreEqual(testArray[1].Name, categoryB.Name);
            Assert.AreEqual(testArray[1].Id, categoryB.Id);
            Assert.AreEqual(testArray[1].SearchString, categoryB.SearchString+",test");
            //check that there are the correct number of records
            Assert.AreEqual(2, testArray.Length);


            //void RemoveCategory(CategoryModel deletedCategory);
            categoryService.RemoveCategory(categoryA);
            testArray = categoryService.GetCategories().ToArray();
            //test that the results are returned in name order
            Assert.AreEqual(testArray[0].Name, categoryB.Name);
            Assert.AreEqual(testArray[0].Id, categoryB.Id);
            Assert.AreEqual(testArray[0].SearchString, categoryB.SearchString+",test");
            //check that there are the correct number of records
            Assert.AreEqual(1, testArray.Length);

        }
    }
}
