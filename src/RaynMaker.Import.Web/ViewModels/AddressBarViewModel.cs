using System;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using RaynMaker.Import.Spec;

namespace RaynMaker.Import.Web.ViewModels
{
    class AddressBarViewModel : BindableBase
    {
        private string myUrl;

        public AddressBarViewModel()
        {
            GoCommand = new DelegateCommand( OnGo );
        }

        public string Url
        {
            get { return myUrl; }
            set { SetProperty( ref myUrl, value ); }
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
    }
}
