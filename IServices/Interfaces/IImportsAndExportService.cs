using Enums;
using Models.Models;

namespace PersonalSpendingAnalysis.IServices
{
    public interface IImportsAndExportService
    {
        ImportResults ImportFile(string fileName);
        string GetExportableText();
        ImportResult ImportJson(string fileText);
    }
}