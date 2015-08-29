using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using RaynMaker.Import.Web.DatumLocationValidation;

namespace RaynMaker.Import.Web.ViewModels
{
    [Export]
    class WebSpyViewModel : BindableBase
    {
        private IBrowser myBrowser;

        public WebSpyViewModel()
        {
            StartValidationCommand = new DelegateCommand( OnStartValidation );

            AddressBar = new AddressBarViewModel();
            Navigation = new NavigationViewModel();
            DataFormat = new DataFormatViewModel();
        }

        public IBrowser Browser
        {
            get { return myBrowser; }
            set
            {
                myBrowser = value;
                AddressBar.Browser = myBrowser;
                Navigation.Browser = myBrowser;
            }
        }

        public ICommand StartValidationCommand { get; private set; }

        private void OnStartValidation()
        {
            var form = new ValidationForm( Browser );
            form.Show();
        }

        public AddressBarViewModel AddressBar { get; private set; }

        public NavigationViewModel Navigation { get; private set; }

        public DataFormatViewModel DataFormat { get; private set; }
    }
}
