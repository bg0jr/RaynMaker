﻿using System.ComponentModel.Composition;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Prism.Commands;
using Prism.Mvvm;
using RaynMaker.Modules.Analysis.Engine;
using RaynMaker.Modules.Analysis.Services;
using RaynMaker.Entities;
using RaynMaker.Infrastructure;
using RaynMaker.Infrastructure.Services;

namespace RaynMaker.Modules.Analysis.ViewModels
{
    [Export]
    class AnalysisContentPageModel : BindableBase, IContentPage
    {
        private Stock myStock;
        private IProjectHost myProjectHost;
        private ICurrenciesLut myCurrenciesLut;
        private StorageService myStorageService;
        private FlowDocument myFlowDocument;

        [ImportingConstructor]
        public AnalysisContentPageModel( IProjectHost projectHost, ILutService lutService, StorageService storageService )
        {
            myProjectHost = projectHost;
            myCurrenciesLut = lutService.CurrenciesLut;
            myStorageService = storageService;

            GoCommand = new DelegateCommand( OnGo );
        }

        public string Header { get { return "Analysis"; } }

        public ICommand GoCommand { get; private set; }

        public FlowDocument Document
        {
            get { return myFlowDocument; }
            set { SetProperty( ref myFlowDocument, value ); }
        }

        private void OnGo()
        {
            var analysisTemplate = myStorageService.LoadAnalysisTemplate( myCurrenciesLut );

            var doc = new FlowDocument();
            doc.Background = Brushes.White;
            doc.FontFamily = new FontFamily( "Arial" );
            doc.FontSize = 13;

            doc.Headline( "{0} (Isin: {1})", myStock.Company.Name, myStock.Isin );

            var context = new ReportContext( myCurrenciesLut, myStock, doc );
            foreach( var element in analysisTemplate.Elements )
            {
                element.Report( context );
            }
            context.Complete();

            Document = doc;
        }

        public void Initialize( Stock stock )
        {
            myStock = stock;

            OnGo();
        }

        public void Cancel() { }

        public void Complete() { }
    }
}
