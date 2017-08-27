using System;
using System.IO;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
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
                Uri uri;
                if( value != null && Uri.TryCreate( value, UriKind.Absolute, out uri ) )
                {
                    if( !uri.IsFile
                        && !value.StartsWith( "file://", StringComparison.OrdinalIgnoreCase )
                        && !value.StartsWith( "http://", StringComparison.OrdinalIgnoreCase )
                        && !value.StartsWith( "https://", StringComparison.OrdinalIgnoreCase ) )
                    {
                        value = "http://" + value;
                    }
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
