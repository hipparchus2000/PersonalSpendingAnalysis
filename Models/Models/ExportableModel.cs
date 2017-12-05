using PersonalSpendingAnalysis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    public class ExportableModel
    {
        //todo change to Model and automap
        public List<TransactionModel> transactions;
        public List<CategoryModel> categories;
    }
}
