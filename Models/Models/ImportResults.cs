using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
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
