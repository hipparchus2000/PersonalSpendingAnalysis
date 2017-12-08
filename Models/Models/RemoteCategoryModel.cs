using System.Collections.Generic;

namespace PersonalSpendingAnalysis.Models
{
    public class RemoteCategoryModel
    {
        public RemoteCategoryModel()
        {
        }

        public bool Success{ get; set; }
        public string ErrorMessage { get; set; }
        public List<dynamic> remoteCategories { get; set; }
    }
}