using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using RaynMaker.Blade.Engine;
using RaynMaker.Blade.Model;
using RaynMaker.Blade.Services;
using RaynMaker.Infrastructure;

namespace RaynMaker.Blade.ViewModels
{
    [Export]
    class AnalysisContentPageModel : BindableBase
    {
        private IProjectHost myProjectHost;
        private StorageService myStorageService;
        private FlowDocument myFlowDocument;

        [ImportingConstructor]
        public AnalysisContentPageModel( IProjectHost projectHost, Project project, StorageService storageService )
        {
            myProjectHost = projectHost;
            Project = project;
            myStorageService = storageService;

            GoCommand = new DelegateCommand( OnGo );

            projectHost.Changed += projectHost_Changed;
            projectHost_Changed();

            OnGo();
        }

        void projectHost_Changed()
        {
            if( myProjectHost.Project != null )
            {
                Project.CurrenciesSheet = myStorageService.LoadCurrencies();
            }
        }

        public string Header { get { return "Analysis"; } }

        public Project Project { get; private set; }

        public ICommand GoCommand { get; private set; }

        public FlowDocument Document
        {
            get { return myFlowDocument; }
            set { SetProperty( ref myFlowDocument, value ); }
        }

        private void OnGo()
        {
            var analysisTemplate = myStorageService.LoadAnalysisTemplate( Project.CurrenciesSheet );
            var dataSheet = myStorageService.LoadDataSheet( Project.DataSheetLocation );

            var doc = new FlowDocument();
            doc.Background = Brushes.White;
            doc.FontFamily = new FontFamily( "Arial" );
            doc.FontSize = 13;

            var stock = dataSheet.Company.Stocks.Single();

            doc.Headline( "{0} (Isin: {1})", stock.Company.Name, stock.Isin );

            var context = new ReportContext( Project, stock, dataSheet, doc );
            foreach( var element in analysisTemplate.Analysis.Elements )
            {
                element.Report( context );
            }
            context.Complete();

            Document = doc;
        }
    }
}
