using Blade.Data;
using Microsoft.Practices.Prism.Mvvm;

namespace RaynMaker.Import.Web.ViewModels
{
    class DataFormatViewModel : BindableBase
    {
        private string myPath;
        private string myValue;
        private CellDimension mySelectedDimension;

        public string Path
        {
            get { return myPath; }
            set { SetProperty( ref myPath, value ); }
        }

        public string Value
        {
            get { return myValue; }
            set { SetProperty( ref myValue, value ); }
        }

        public CellDimension SelectedDimension
        {
            get { return mySelectedDimension; }
            set { SetProperty( ref mySelectedDimension, value ); }
        }

    }
}
