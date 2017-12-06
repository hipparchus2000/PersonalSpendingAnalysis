using System.Windows.Forms;

namespace PersonalSpendingAnalysis.IServices
{
    public interface IReportService
    {
        void createReport(TreeNodeCollection nodes, string filename, bool includeTransactions);
    }
}