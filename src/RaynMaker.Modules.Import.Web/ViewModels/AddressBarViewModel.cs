using System;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using RaynMaker.Modules.Import.Spec;
using RaynMaker.Modules.Import.Spec.v2;
using RaynMaker.Modules.Import.Spec.v2.Locating;

namespace RaynMaker.Modules.Import.Web.ViewModels
{
    class AddressBarViewModel : BindableBase
    {
        private string myUrl;

        public AddressBarViewModel()
        {
            GoCommand = new DelegateCommand( OnGo );
            ClearCacheCommand = new DelegateCommand( OnClearCache );
        }

        public string Url
        {
            get { return myUrl; }
            set
            {
                if( value != null
                    && !value.StartsWith( "http://", StringComparison.OrdinalIgnoreCase )
                    && !value.StartsWith( "https://", StringComparison.OrdinalIgnoreCase ) )
                {
                    value = "http://" + value;
                }
                SetProperty( ref myUrl, value );
            }
        }

        public IDocumentBrowser Browser { get; set; }

        public ICommand GoCommand { get; private set; }

        private void OnGo()
        {
            if( string.IsNullOrEmpty( myUrl ) )
            {
                return;
            }

            Browser.Navigate( DocumentType.Html, new Uri( myUrl ) );
        }

        public ICommand ClearCacheCommand { get; private set; }

        private void OnClearCache()
        {
            Browser.ClearCache();
        }
    }
}
