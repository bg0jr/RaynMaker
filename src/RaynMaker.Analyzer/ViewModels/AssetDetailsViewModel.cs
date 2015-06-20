using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Regions;
using RaynMaker.Analyzer.Services;

namespace RaynMaker.Analyzer.ViewModels
{
    [Export]
    class AssetDetailsViewModel : BindableBase, INavigationAware
    {
        private string myHeader;

        [ImportingConstructor]
        public AssetDetailsViewModel()
        {
        }

        public string Header
        {
            get { return myHeader; }
            private set { SetProperty( ref myHeader, value ); }
        }

        public bool IsNavigationTarget( NavigationContext navigationContext )
        {
            return true;
        }

        public void OnNavigatedFrom( NavigationContext navigationContext )
        {
        }

        public void OnNavigatedTo( NavigationContext navigationContext )
        {
            var args = new AssetNavigationParameters( navigationContext.Parameters );
            Header = string.Format( "Asset {0}", args.AssetId );
        }
    }
}
