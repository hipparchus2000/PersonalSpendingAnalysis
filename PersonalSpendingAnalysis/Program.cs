using IServices.Interfaces;
using PersonalSpendingAnalysis.IServices;
using PersonalSpendingAnalysis.Services;
using Services.Services;
using System;
using System.Windows.Forms;
using Unity;

namespace PersonalSpendingAnalysis
{
    static class Program
    {
        static IUnityContainer container = new UnityContainer();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            InjectDependencies();

            var importsAndExportsService = container.Resolve<IImportsAndExportService>();
            var queryService = container.Resolve<IQueryService>();
            var budgetsService = container.Resolve<IBudgetsService>();
            var categoryService = container.Resolve<ICategoryService>();
            var transactionService = container.Resolve<ITransactionService>();
            var reportService = container.Resolve<IReportService>();
            
            Application.Run(new PersonalSpendingAnalysis(importsAndExportsService, budgetsService, queryService,categoryService,transactionService, reportService));
        }

        private static void InjectDependencies()
        {
            container.RegisterType<IQueryService, QueryService>();
            container.RegisterType<IBudgetsService, BudgetsService>();
            container.RegisterType<IImportsAndExportService, ImportsAndExportService>();
            container.RegisterType<ICategoryService, CategoryService>();
            container.RegisterType<ITransactionService, TransactionService>();
            container.RegisterType<IReportService, ReportService>();
        }
    }
}
