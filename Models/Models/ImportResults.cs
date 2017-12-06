using System;

namespace PersonalSpendingAnalysis.Models
{
    public class ImportResults
    {
        public ImportResults()
        {
            importId = Guid.NewGuid();
        }
        public Guid importId { get; set; }
        public int NumberOfRecordsImported { get; set; }
        public int NumberOfDuplicatesFound { get; set; }
        public int NumberOfNewRecordsFound { get; set; }
        public int NumberOfFieldsFound { get; set; }
    }
}
