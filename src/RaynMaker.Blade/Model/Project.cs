using System.ComponentModel.Composition;
using System.IO;
using Microsoft.Practices.Prism.Mvvm;
using RaynMaker.Blade.Entities;
using RaynMaker.Infrastructure;

namespace RaynMaker.Blade.Model
{
    [Export]
    class Project : BindableBase
    {
        private IProjectHost myProjectHost;
        private string myCurrenciesSheetLocation;
        private string myAnalysisTemplateLocation;
        private string myDataSheetLocation;
        private CurrenciesSheet myCurrenciesSheet;

        [ImportingConstructor]
        public Project( IProjectHost projectHost )
        {
            myProjectHost = projectHost;
            myProjectHost.Changed += myProjectHost_Changed;
        }

        void myProjectHost_Changed()
        {
            OnPropertyChanged( () => CurrenciesSheetLocation );
            OnPropertyChanged( () => AnalysisTemplateLocation );
            OnPropertyChanged( () => DataSheetLocation );
        }

        public string CurrenciesSheetLocation
        {
            get
            {
                if( myProjectHost.Project == null )
                {
                    return null;
                }
                if( myCurrenciesSheetLocation == null && myProjectHost.Project.UserData.ContainsKey( "CurrenciesSheetLocation" ) )
                {
                    myCurrenciesSheetLocation = myProjectHost.Project.UserData[ "CurrenciesSheetLocation" ];
                }
                if( myCurrenciesSheetLocation == null )
                {
                    CurrenciesSheetLocation = Path.Combine( Path.GetDirectoryName( GetType().Assembly.Location ), "Resources", "Currencies.xdb" );
                }
                return myCurrenciesSheetLocation;
            }
            set
            {
                if( SetProperty( ref myCurrenciesSheetLocation, value != null ? value.Trim() : value ) )
                {
                    myProjectHost.Project.UserData[ "CurrenciesSheetLocation" ] = myCurrenciesSheetLocation;
                    myProjectHost.Project.IsDirty = true;
                }
            }
        }

        public string AnalysisTemplateLocation
        {
            get
            {
                if( myProjectHost.Project == null )
                {
                    return null;
                }
                if( myAnalysisTemplateLocation == null && myProjectHost.Project.UserData.ContainsKey( "AnalysisTemplateLocation" ) )
                {
                    myAnalysisTemplateLocation = myProjectHost.Project.UserData[ "AnalysisTemplateLocation" ];
                }
                return myAnalysisTemplateLocation;
            }
            set
            {
                if( SetProperty( ref myAnalysisTemplateLocation, value != null ? value.Trim() : value ) )
                {
                    myProjectHost.Project.UserData[ "AnalysisTemplateLocation" ] = myAnalysisTemplateLocation;
                    myProjectHost.Project.IsDirty = true;
                }
            }
        }

        public string DataSheetLocation
        {
            get
            {
                if( myProjectHost.Project == null )
                {
                    return null;
                }
                if( myDataSheetLocation == null && myProjectHost.Project.UserData.ContainsKey( "DataSheetLocation" ) )
                {
                    myDataSheetLocation = myProjectHost.Project.UserData[ "DataSheetLocation" ];
                }
                return myDataSheetLocation;
            }
            set
            {
                if( SetProperty( ref myDataSheetLocation, value != null ? value.Trim() : value ) )
                {
                    myProjectHost.Project.UserData[ "DataSheetLocation" ] = myDataSheetLocation;
                    myProjectHost.Project.IsDirty = true;
                }
            }
        }

        public CurrenciesSheet CurrenciesSheet
        {
            get { return myCurrenciesSheet; }
            set
            {
                if( SetProperty( ref myCurrenciesSheet, value ) )
                {
                    CurrencyConverter.Sheet = myCurrenciesSheet;
                }
            }
        }
    }
}
