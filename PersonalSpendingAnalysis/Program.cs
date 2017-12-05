using AutoMapper;
using PersonalSpendingAnalysis.IServices;
using PersonalSpendingAnalysis.Services;
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
            var aggregatesService = container.Resolve<IAggregatesService>();

            Application.Run(new PersonalSpendingAnalysis(importsAndExportsService,aggregatesService,queryService));
        }

        private static void InjectDependencies()
        {
            container.RegisterType<IQueryService, QueryService>();
            container.RegisterType<IAggregatesService, AggregatesService>();
            container.RegisterType<IImportsAndExportService, ImportsAndExportService>();
            
        }
    }
}
