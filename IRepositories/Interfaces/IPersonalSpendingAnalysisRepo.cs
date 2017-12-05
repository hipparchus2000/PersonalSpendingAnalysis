using PersonalSpendingAnalysis.Services;

namespace IRepositories.Interfaces
{
    public interface IPersonalSpendingAnalysisRepo
    {
        //add in public methods
        System.Collections.Generic.List<TransactionDto> GetTransactions();
        System.Collections.Generic.List<CategoryDto> GetCategories();
        TransactionDto GetTransaction(string id);
        void AddTransaction(TransactionDto dto);
    }
}
