using IRepositories.Interfaces;
using IServices.Interfaces;
using PersonalSpendingAnalysis.IServices;
using PersonalSpendingAnalysis.Services;
using Services.Services;
using Unity;
using Repositories;

namespace UnitTests.ServiceTests
{
    public abstract class ServiceTestBaseClass
    {
        static IUnityContainer container = new UnityContainer();
        public IPersonalSpendingAnalysisRepo personalSpendingAnalysisRepo;
        public IQueryService queryService;
        public IBudgetsService budgetsService;
        public ICategoryService categoryService;
        public ITransactionService transactionService;
        public IReportService reportService;
        public ImportsAndExportService importsAndExportService;

        public ServiceTestBaseClass()
        {

            InjectDependencies();

            //resolve concrete types
            //var importsAndExportsService = container.Resolve<ImportsAndExportService>();
            personalSpendingAnalysisRepo = container.Resolve<FakeRepo>();
            queryService = container.Resolve<QueryService>();
            importsAndExportService = container.Resolve<ImportsAndExportService>();
            budgetsService = container.Resolve<BudgetsService>();
            categoryService = container.Resolve<CategoryService>();
            transactionService = container.Resolve<TransactionService>();
            reportService = container.Resolve<ReportService>();

            personalSpendingAnalysisRepo.ClearFakeRepo();
        }

        private static void InjectDependencies()
        {
            container.RegisterType<IPersonalSpendingAnalysisRepo, FakeRepo>();
            container.RegisterType<IQueryService, QueryService>();
            container.RegisterType<IBudgetsService, BudgetsService>();
            //todo fix this
            container.RegisterType<IImportsAndExportService, ImportsAndExportService>();
            container.RegisterType<ICategoryService, CategoryService>();
            container.RegisterType<ITransactionService, TransactionService>();
            container.RegisterType<IReportService, ReportService>();
        }


    }
}
