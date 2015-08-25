using System.ComponentModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using RaynMaker.Blade.Engine;
using RaynMaker.Blade.Model;
using RaynMaker.Blade.Services;
using RaynMaker.Entities;
using RaynMaker.Infrastructure;

namespace RaynMaker.Blade.ViewModels
{
    [Export]
    class AnalysisContentPageModel : BindableBase, IContentPage
    {
        private Stock myStock;
        private IProjectHost myProjectHost;
        private StorageService myStorageService;
        private FlowDocument myFlowDocument;

        [ImportingConstructor]
        public AnalysisContentPageModel( IProjectHost projectHost, Project project, StorageService storageService )
        {
            myProjectHost = projectHost;
            Project = project;
            myStorageService = storageService;

            GoCommand = new DelegateCommand( OnGo, CanGo );

            projectHost.Changed += projectHost_Changed;
            projectHost_Changed();
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

        public DelegateCommand GoCommand { get; private set; }

        public FlowDocument Document
        {
            get { return myFlowDocument; }
            set { SetProperty( ref myFlowDocument, value ); }
        }

        private bool CanGo()
        {
            return myStock != null && File.Exists( myStock.Company.XdbPath );
        }

        private void OnGo()
        {
            if( !File.Exists( myStock.Company.XdbPath ) )
            {
                return;
            }

            var analysisTemplate = myStorageService.LoadAnalysisTemplate( Project.CurrenciesSheet );
            var dataSheet = myStorageService.LoadDataSheet( myStock.Company.XdbPath );

            var doc = new FlowDocument();
            doc.Background = Brushes.White;
            doc.FontFamily = new FontFamily( "Arial" );
            doc.FontSize = 13;

            doc.Headline( "{0} (Isin: {1})", myStock.Company.Name, myStock.Isin );

            var context = new ReportContext( Project, myStock, dataSheet, doc );
            foreach( var element in analysisTemplate.Analysis.Elements )
            {
                element.Report( context );
            }
            context.Complete();

            Document = doc;
        }

        public void Initialize( Stock stock )
        {
            myStock = stock;
            PropertyChangedEventManager.AddHandler( myStock, OnXdbPathChanged, PropertySupport.ExtractPropertyName( () => myStock.Company.XdbPath ) );

            OnGo();
        }

        private void OnXdbPathChanged( object sender, PropertyChangedEventArgs e )
        {
            GoCommand.RaiseCanExecuteChanged();
            OnGo();
        }

        public void Cancel() { }

        public void Complete() { }
    }
}
