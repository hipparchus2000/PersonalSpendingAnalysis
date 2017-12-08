using System.Collections.Generic;

namespace PersonalSpendingAnalysis.Models
{
    public class RemoteTransactionModel
    {
        public RemoteTransactionModel()
        {
        }

        public bool Success{ get; set; }
        public string ErrorMessage { get; set; }
        public List<dynamic> remoteTransactions { get; set; }
    }
}