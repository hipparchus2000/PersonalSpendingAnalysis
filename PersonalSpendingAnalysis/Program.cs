using IRepositories.Interfaces;
using IServices.Interfaces;
using PersonalSpendingAnalysis.IServices;
using PersonalSpendingAnalysis.Repo;
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

            //resolve concrete types
            var importsAndExportsService = container.Resolve<ImportsAndExportService>();
            var queryService = container.Resolve<QueryService>();
            var budgetsService = container.Resolve<BudgetsService>();
            var categoryService = container.Resolve<CategoryService>();
            var transactionService = container.Resolve<TransactionService>();
            var reportService = container.Resolve<ReportService>();
            var psaContext = container.Resolve<PSAContext>();

            Application.Run(new PersonalSpendingAnalysis(importsAndExportsService, budgetsService, queryService,categoryService,transactionService, reportService));
        }

        private static void InjectDependencies()
        { 
            container.RegisterType<IPersonalSpendingAnalysisRepo, PersonalSpendingAnalysisRepo>();
            container.RegisterType<IQueryService, QueryService>();
            container.RegisterType<IBudgetsService, BudgetsService>();
            container.RegisterType<IImportsAndExportService, ImportsAndExportService>();
            container.RegisterType<ICategoryService, CategoryService>();
            container.RegisterType<ITransactionService, TransactionService>();
            container.RegisterType<IReportService, ReportService>();
        }
    }
}
