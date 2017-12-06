using System;

namespace PersonalSpendingAnalysis.Dtos
{
    public class CategoryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string SearchString { get; set; }
    }
}