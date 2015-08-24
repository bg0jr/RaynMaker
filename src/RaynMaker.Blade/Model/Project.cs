using System.ComponentModel.Composition;
using System.IO;
using Microsoft.Practices.Prism.Mvvm;
using Plainion;
using RaynMaker.Blade.Entities;
using RaynMaker.Infrastructure;

namespace RaynMaker.Blade.Model
{
    [Export]
    class Project : BindableBase
    {
        private IProjectHost myProjectHost;
        private string myDataSheetLocation;
        private CurrenciesSheet myCurrenciesSheet;
        private int myMaxCurrencyTranslationsAgeInDays;

        [ImportingConstructor]
        public Project( IProjectHost projectHost )
        {
            myProjectHost = projectHost;
            myProjectHost.Changed += myProjectHost_Changed;

            myMaxCurrencyTranslationsAgeInDays = -1;
        }

        void myProjectHost_Changed()
        {
            OnPropertyChanged( () => DataSheetLocation );
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
            set { SetProperty( ref myCurrenciesSheet, value ); }
        }

        public int MaxCurrencyTranslationsAgeInDays
        {
            get
            {
                Contract.RequiresNotNull( myProjectHost.Project, "myProjectHost.Project" );

                if( myMaxCurrencyTranslationsAgeInDays == -1 )
                {
                    if( myProjectHost.Project.UserData.ContainsKey( "MaxCurrencyTranslationsAgeInDays" ) )
                    {
                        myMaxCurrencyTranslationsAgeInDays = int.Parse( myProjectHost.Project.UserData[ "MaxCurrencyTranslationsAgeInDays" ] );
                    }
                    else
                    {
                        MaxCurrencyTranslationsAgeInDays = 7;
                    }
                }

                return myMaxCurrencyTranslationsAgeInDays;
            }
            set
            {
                if( SetProperty( ref myMaxCurrencyTranslationsAgeInDays, value ) )
                {
                    myProjectHost.Project.UserData[ "MaxCurrencyTranslationsAgeInDays" ] = myMaxCurrencyTranslationsAgeInDays.ToString();
                    myProjectHost.Project.IsDirty = true;
                }
            }
        }

    }
}
