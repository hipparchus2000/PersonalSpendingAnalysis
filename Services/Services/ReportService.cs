using System.Windows.Forms;
using PersonalSpendingAnalysis.IServices;
using System.Collections.Generic;
using System;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;

namespace Services.Services
{
    public class ReportService : IReportService
    {
        const String indent = "    ";

        private void walkTree(TreeNodeCollection nodes, string spaces, ref List<string> s, int indentLevel, int maxIndentLevel)
        {
            spaces = spaces + indent;
            indentLevel++;

            foreach (TreeNode node in nodes)
            {
                s.Add(spaces + node.Text);
                if (node.Nodes.Count > 0 && indentLevel <= maxIndentLevel)
                    walkTree(node.Nodes, spaces, ref s, indentLevel, maxIndentLevel);
            }
        }

        public void createReport(TreeNodeCollection nodes, string filename, bool includeTransactions)
        {
            List<string> s = new List<string>();
            string spaces = "";
            int indentLevel = 0;
            int maxIndentLevel = 5;

            if (!includeTransactions)
            {
                maxIndentLevel = 2;
            }

            walkTree(nodes, spaces, ref s, indentLevel, maxIndentLevel);

            var pagesize = 70;
            var numberOfPages = (int)(s.Count / pagesize) + 1;

            PdfDocument document = new PdfDocument();
            XFont categoryfont = new XFont("Arial", 8, XFontStyle.Bold);
            XFont yearfont = new XFont("Arial", 8, XFontStyle.Underline);
            XFont font = new XFont("Arial", 8, XFontStyle.Regular);

            for (var pageNum = 0; pageNum < numberOfPages; pageNum++)
            {
                var numberOfRows = pagesize;
                if (((pageNum * pagesize) + pagesize) > s.Count)
                    numberOfRows = s.Count % pagesize;
                string text = String.Join("\r\n", s.GetRange(pageNum * pagesize, numberOfRows));

                PdfPage page = document.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(page);
                XTextFormatter tf = new XTextFormatter(gfx);

                XRect rect = new XRect(20, 50, 550, 850);
                gfx.DrawRectangle(XBrushes.White, rect);
                tf.Alignment = XParagraphAlignment.Left;
                tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);
            }

            document.Save(filename);

        }
    }
}
