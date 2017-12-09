using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests.ServiceTests;

namespace UnitTests
{
    [TestClass]
    public class ImportsAndExportService : ServiceTestBaseClass
    {
        //these bits are immediately testable
        //ImportResults ImportFile(string fileName);
        //string GetExportableText();
        //ImportResult ImportJson(string fileText);
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
        public void TestMethod1()
        {
        }
    }
}
