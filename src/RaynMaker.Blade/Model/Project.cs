using System.ComponentModel.Composition;
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
        private CurrenciesSheet myCurrenciesSheet;
        private int myMaxCurrencyTranslationsAgeInDays;

        [ImportingConstructor]
        public Project( IProjectHost projectHost )
        {
            myProjectHost = projectHost;

            myMaxCurrencyTranslationsAgeInDays = -1;
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
