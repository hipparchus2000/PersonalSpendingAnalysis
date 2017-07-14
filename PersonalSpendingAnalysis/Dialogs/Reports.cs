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
using System.Windows.Forms.DataVisualization.Charting;
using System.Globalization;

namespace PersonalSpendingAnalysis.Dialogs
{
    
    public partial class Reports : Form
    {
        SeriesChartType style;

        static string[] ColourValues = new string[] {
            "FF0000", "00FF00", "0000FF", "FFFF00", "FF00FF", "00FFFF", "000000",
            "800000", "008000", "000080", "808000", "800080", "008080", "808080",
            "C00000", "00C000", "0000C0", "C0C000", "C000C0", "00C0C0", "C0C0C0",
            "400000", "004000", "000040", "404000", "400040", "004040", "404040",
            "200000", "002000", "000020", "202000", "200020", "002020", "202020",
            "600000", "006000", "000060", "606000", "600060", "006060", "606060",
            "A00000", "00A000", "0000A0", "A0A000", "A000A0", "00A0A0", "A0A0A0",
            "E00000", "00E000", "0000E0", "E0E000", "E000E0", "00E0E0", "E0E0E0",
        };

        public Reports()
        {
            InitializeComponent();
        }

        private void listOfChartTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            drawChart();
        }

        public static IEnumerable<Tuple<int, int>> MonthsBetween(
            DateTime startDate,
            DateTime endDate)
        {
            DateTime iterator;
            DateTime limit;

            if (endDate > startDate)
            {
                iterator = new DateTime(startDate.Year, startDate.Month, 1);
                limit = endDate;
            }
            else
            {
                iterator = new DateTime(endDate.Year, endDate.Month, 1);
                limit = startDate;
            }

            var dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat;
            while (iterator <= limit)
            {
                yield return Tuple.Create(iterator.Month,iterator.Year);
                iterator = iterator.AddMonths(1);
            }
        }

        private void drawChart()
        {
            
            var context = new PersonalSpendingAnalysisRepo();
            var transactions = context.Transaction.Include("Category")
                .Where(x => (x.transactionDate > this.startDate.Value) 
                && (x.transactionDate < this.endDate.Value)
                && (this.showDebitsOnly.Checked && x.amount<0)
                );
            var categories = transactions
                .GroupBy(x => new { Year = x.transactionDate.Year, Month = x.transactionDate.Month, CategoryName = x.Category.Name } )
                .Select(x => new {
                    Year = x.Key.Year,
                    Month = x.Key.Month,
                    CategoryName = x.Key.CategoryName,
                    Amount = -1 * x.Sum(y => y.amount) })
                    .ToList();

            this.chart.Series.Clear();
            this.chart.Legends.Clear();
            chart.ChartAreas.First().AxisX.CustomLabels.Clear();

            List <String> categoryList = categories.Select(x => x.CategoryName).Distinct().ToList();
            var monthList = MonthsBetween(this.startDate.Value, this.endDate.Value);

            var monthnum = 1;
            foreach (var month in monthList)
            {
                var yearmonth = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(month.Item1) + " " + month.Item2;
                chart.ChartAreas.First().AxisX.CustomLabels.Add(monthnum-0.5, monthnum+0.5, yearmonth);
                monthnum++;
            }

            var colorIndex = 0;

            foreach (var category in categoryList)
            {
                var newColor = ColorTranslator.FromHtml("#"+ColourValues[colorIndex]);
                var subSet = categories.Where(x => x.CategoryName == category).OrderBy(x=>x.Month);
                var newSeries = new Series()
                {
                    Name = category==null?"uncategorized":category,
                    Color = newColor,
                    IsVisibleInLegend = true,
                    IsXValueIndexed = true,
                    ChartType = style,
                };

                foreach (var month in monthList)
                {
                    var thisMonth = subSet.FirstOrDefault(x => (x.Month==month.Item1) && (x.Year==month.Item2));
                    var total = thisMonth == null ? 0 : thisMonth.Amount;
                    newSeries.Points.AddY((double)total);
                }

                this.chart.Series.Add(newSeries);
                this.chart.Legends.Add(new Legend (category));
                colorIndex++;
            }

        }

        private void Charts_Load(object sender, EventArgs e)
        {
            var startDate = DateTime.Today.AddYears(-1);
            this.endDate.Value = DateTime.Today;
            this.startDate.Value = startDate;
            this.showDebitsOnly.Checked = true;
            style = SeriesChartType.StackedColumn;
            drawChart();
        }

        private void startDate_ValueChanged(object sender, EventArgs e)
        {
            drawChart();
        }

        private void endDate_ValueChanged(object sender, EventArgs e)
        {
            drawChart();
        }

        private void showDebitsOnly_CheckedChanged(object sender, EventArgs e)
        {
            drawChart();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            drawChart();
        }

        private void chartType2_CheckedChanged(object sender, EventArgs e)
        {

            drawChart();
        }

        private void chartType3_CheckedChanged(object sender, EventArgs e)
        {

            drawChart();
        }

        private void chartType4_CheckedChanged(object sender, EventArgs e)
        {
            drawChart();
        }

        private void radioButtonStackedColumn_CheckedChanged(object sender, EventArgs e)
        {
            style = SeriesChartType.StackedColumn;
            drawChart();
        }

        private void radioButtonStackedArea_CheckedChanged(object sender, EventArgs e)
        {
            style = SeriesChartType.StackedArea;
            drawChart();
        }

        private void chart_Click(object sender, EventArgs e)
        {

        }
    }
}
