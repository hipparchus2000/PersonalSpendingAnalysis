using Enums;
using PersonalSpendingAnalysis.Models;
using System;

namespace PersonalSpendingAnalysis.IServices
{
    public interface IImportsAndExportService
    {
        ImportResults ImportCsv(string csvText);
        string GetExportableText();
        ImportResult ImportJson(string fileText);
        LoginResult LoginToWebService(string username, string password);

        RemoteCategoryModel GetRemoteCategories(LoginResult loginResult);
        bool DeleteRemoteCategory(LoginResult loginResult, Guid id);
        bool PostNewCategoryToRemote(LoginResult loginResult, object localCategory);
        CategoryModel ExtractCategoryModelFromJson(string stripped);

        RemoteTransactionModel GetRemoteTransactions(LoginResult loginResult);
        bool DeleteRemoteTransaction(LoginResult loginResult, Guid id);
        bool PostNewTransactionToRemote(LoginResult loginResult, object localCategory);
        TransactionModel ExtractTransactionModelFromJson(string stripped);

    }
}