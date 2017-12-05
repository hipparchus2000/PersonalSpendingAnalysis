using Models.Models;

namespace PersonalSpendingAnalysis.IServices
{
    public interface IImportsAndExportService
    {
        ImportResults ImportFile(string fileName);
        string GetExportableText();
    }
}