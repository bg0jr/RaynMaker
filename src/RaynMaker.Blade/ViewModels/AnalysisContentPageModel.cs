using System.ComponentModel.Composition;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
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

        [ImportingConstructor]
        public AnalysisContentPageModel( IProjectHost projectHost, Project project, StorageService storageService )
        {
            myProjectHost = projectHost;
            Project = project;
            myStorageService = storageService;

            GoCommand = new DelegateCommand( OnGo );

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

        public ICommand GoCommand { get; private set; }

        private void OnGo()
        {
            var analysisTemplate = myStorageService.LoadAnalysisTemplate( Project.CurrenciesSheet );
            var dataSheet = myStorageService.LoadDataSheet( Project.DataSheetLocation );

            var analyzer = new StockAnalyzer( Project, analysisTemplate.Analysis );
            analyzer.Execute( dataSheet );
        }
    }
}
