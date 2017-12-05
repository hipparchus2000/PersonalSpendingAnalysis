using System;


namespace PersonalSpendingAnalysis.Models
{
    public class TransactionModel
    {
        public string _id { get; set; }
        public Guid Id { get; set; }
        public decimal amount { get; set; }
        public DateTime transactionDate { get; set; }
        public string Notes { get; set; }
        public Guid? CategoryId { get; set; }
        public String SubCategory { get; set; } //subcategory is search string found
        public Guid? AccountId { get; set; }
        public String SHA256 { get; set; }
        public bool ManualCategory { get; set; }
        public string userId { get; internal set; }
    }
}
