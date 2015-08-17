using System.ComponentModel.Composition;
using Microsoft.Practices.Prism.Mvvm;

namespace RaynMaker.Blade.Model
{
    [Export]
    class Project : BindableBase
    {
        private string myCurrenciesSheetLocation;
        private string myAnalysisTemplateLocation;
        private string myDataSheetLocation;

        public string CurrenciesSheetLocation
        {
            get { return myCurrenciesSheetLocation; }
            set { SetProperty( ref myCurrenciesSheetLocation, value != null ? value.Trim() : value ); }
        }

        public string AnalysisTemplateLocation
        {
            get { return myAnalysisTemplateLocation; }
            set { SetProperty( ref myAnalysisTemplateLocation, value != null ? value.Trim() : value ); }
        }

        public string DataSheetLocation
        {
            get { return myDataSheetLocation; }
            set { SetProperty( ref myDataSheetLocation, value != null ? value.Trim() : value ); }
        }
    }
}
