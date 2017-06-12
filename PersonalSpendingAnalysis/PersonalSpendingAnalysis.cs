using PersonalSpendingAnalysis.Repo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PersonalSpendingAnalysis
{
    public partial class PersonalSpendingAnalysis : Form
    {

        public PersonalSpendingAnalysis()
        {
            InitializeComponent();
        }

        private void ImportCsv_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                string csvText = System.IO.File.ReadAllText(openFileDialog1.FileName);
                csvText = csvText.Replace("\n", "");
                var importLines = csvText.Split('\r');
                foreach (var importLine in importLines)
                {
                    var columns = importLine.Split(',');
                    var context = new PersonalSpendingAnalysisRepo();

                    DateTime tDate;
                    DateTime.TryParse(columns[0], out tDate);
                    decimal tAmount;
                    Decimal.TryParse(columns[3], out tAmount);

                    context.Transaction.Add(new Repo.Entities.Transaction
                    {
                        transactionDate = tDate,
                        amount = tAmount,
                        Notes = columns[2],
                        Category = null
                    });
                }


                
            }
        }
    }
}
