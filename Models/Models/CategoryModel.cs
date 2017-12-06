using System;

namespace PersonalSpendingAnalysis.Models
{
    public class CategoryModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string SearchString { get; set; }
    }
}