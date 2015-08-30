using Blade.Data;
using Microsoft.Practices.Prism.Mvvm;

namespace RaynMaker.Import.Web.ViewModels
{
    class DataFormatViewModel : BindableBase
    {
        private string myPath;
        private string myValue;
        private CellDimension mySelectedDimension;
        private string mySeriesName;
        private bool myIsValid;
        private string myRowHeaderColumn;
        private string mySkipRows;

        public DataFormatViewModel()
        {
            IsValid = true;
        }

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

        public string SeriesName
        {
            get { return mySeriesName; }
            set { SetProperty( ref mySeriesName, value ); }
        }

        public bool IsValid
        {
            get { return myIsValid; }
            set { SetProperty( ref myIsValid, value ); }
        }

        public string RowHeaderColumn
        {
            get { return myRowHeaderColumn; }
            set { SetProperty( ref myRowHeaderColumn, value ); }
        }

        public string SkipRows
        {
            get { return mySkipRows; }
            set { SetProperty( ref mySkipRows, value ); }
        }
    }
}
