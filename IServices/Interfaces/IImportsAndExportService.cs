using Enums;
using PersonalSpendingAnalysis.Models;

namespace PersonalSpendingAnalysis.IServices
{
    public interface IImportsAndExportService
    {
        ImportResults ImportFile(string fileName);
        string GetExportableText();
        ImportResult ImportJson(string fileText);
    }
}