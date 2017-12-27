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
    public class CategoryServiceMoqTest
    {
        [TestMethod]

        public void GetCategories() {
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
            var list = new List<CategoryDto>();
            list.Add(categoryB);
            list.Add(categoryA);
            var repo = new Mock<IPersonalSpendingAnalysisRepo>();
            repo.Setup(x => x.GetCategories()).Returns(list);

            var service = new CategoryService(repo.Object);
            service.Should().NotBeNull();

            //test
            var result = service.GetCategories().ToArray();

            //assert
            result.Should().NotBeNull();
            result.Length.Should().Be(2);
            result[0].Name.Should().Be(categoryB.Name);
            result[0].Id.Should().Be(categoryB.Id);
            result[0].SearchString.Should().Be(categoryB.SearchString);
            result[1].Name.Should().Be(categoryA.Name);
            result[1].Id.Should().Be(categoryA.Id);
            result[1].SearchString.Should().Be(categoryA.SearchString);
        }

        [TestMethod]
        public void GetListOfCategories()
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
            var list = new List<CategoryDto>();
            list.Add(categoryB);
            list.Add(categoryA);
            var names = list.Select(x => x.Name).ToList();

            var repo = new Mock<IPersonalSpendingAnalysisRepo>();
            repo.Setup(x => x.GetCategories()).Returns(list);
            repo.Setup(x => x.GetCategoryNames()).Returns(names);

            var service = new CategoryService(repo.Object);
            service.Should().NotBeNull();

            //test
            var result = service.GetListOfCategories().ToArray();

            //assert
            result.Should().NotBeNull();
            result.Length.Should().Be(2);
            result[0].Should().Be(categoryB.Name);
            result[1].Should().Be(categoryA.Name);
            
        }

        [TestMethod]
        public void AddNewCategory()
        {
            //Setup
            var category = new CategoryModel
            {
                Id = Guid.NewGuid(),
                Name = "testCat",
                SearchString = "testString1, testString2"
            };
            var repo = new Mock<IPersonalSpendingAnalysisRepo>();
            var service = new CategoryService(repo.Object);
            service.Should().NotBeNull();

            //Test
            service.AddNewCategory(category);

            //Assert
            repo.Verify(x => x.AddNewCategory(It.IsAny<CategoryDto>()), Times.Once);
            repo.Verify(x => x.AddNewCategory(It.Is<CategoryDto>(l => l.Id == category.Id)), Times.Once);
            repo.Verify(x => x.AddNewCategory(It.Is<CategoryDto>(l => l.Name.Equals("testCat"))), Times.Once);
            repo.Verify(x => x.AddNewCategory(It.Is<CategoryDto>(l => l.SearchString.Equals("testString1, testString2"))), Times.Once);
        }

        [TestMethod]
        public void RemoveCategory()
        {
            //Setup
            var category = new CategoryModel
            {
                Id = Guid.NewGuid(),
                Name = "testCat",
                SearchString = "testString1, testString2"
            };
            var repo = new Mock<IPersonalSpendingAnalysisRepo>();
            var service = new CategoryService(repo.Object);
            service.Should().NotBeNull();

            //Test
            service.RemoveCategory(category);

            //Assert
            repo.Verify(x => x.RemoveCategory(It.IsAny<CategoryDto>()), Times.Once);
            repo.Verify(x => x.RemoveCategory(It.Is<CategoryDto>(l => l.Id == category.Id)), Times.Once);
            repo.Verify(x => x.RemoveCategory(It.Is<CategoryDto>(l => l.Name.Equals("testCat"))), Times.Once);
            repo.Verify(x => x.RemoveCategory(It.Is<CategoryDto>(l => l.SearchString.Equals("testString1, testString2"))), Times.Once);
        }

        [TestMethod]
        public void UpdateCategorySearchString()
        {
            //Setup
            var category = new CategoryModel
            {
                Id = Guid.NewGuid(),
                Name = "testCat",
                SearchString = "testString1, testString2"
            };
            var repo = new Mock<IPersonalSpendingAnalysisRepo>();
            var service = new CategoryService(repo.Object);
            service.Should().NotBeNull();

            //Test
            service.UpdateCategorySearchString(category.Id,category.SearchString);

            //Assert
            repo.Verify(x => x.UpdateCategorySearchString(It.IsAny<Guid>(),It.IsAny<string>()), Times.Once);
            repo.Verify(x => x.UpdateCategorySearchString(It.Is<Guid>(l => l == category.Id),It.Is<string>(l=>l == category.SearchString)), Times.Once);
        }

    }
}
