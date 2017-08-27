using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using Prism.Mvvm;
using Plainion;
using RaynMaker.Entities;
using RaynMaker.Infrastructure;
using RaynMaker.Infrastructure.Services;

namespace RaynMaker.Data.Services
{
    class CurrenciesLut : BindableBase, ICurrenciesLut
    {
        private IProjectHost myProjectHost;
        private int myMaxCurrencyTranslationsAgeInDays;

        public CurrenciesLut( IProjectHost projectHost )
        {
            myProjectHost = projectHost;

            Currencies = new ObservableCollection<Currency>();
            myMaxCurrencyTranslationsAgeInDays = -1;
        }

        public ObservableCollection<Currency> Currencies { get; private set; }

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

        public void Reload()
        {
            myMaxCurrencyTranslationsAgeInDays = -1;
            Currencies.Clear();

            var ctx = myProjectHost.Project.GetAssetsContext();
            if( !ctx.Currencies.Any() )
            {
                ctx.Currencies.Add( new Currency { Symbol = "EUR", Name = "Euro" } );
                ctx.Currencies.Add( new Currency { Symbol = "USD", Name = "U.S. Dollar" } );

                ctx.SaveChanges();
            }

            // first load all currencies so that when loading the translations all currencies are loaded and so a translation
            // can be initialized completely before setting IsMaterialized = true.
            // without that we face the problem that IsMaterialized = true is set BEFORE the Target is set. This causes
            // unwanted Timestamp updates
            ctx.Currencies.Load();

            foreach( var currency in ctx.Currencies.Include( c => c.Translations ) )
            {
                Currencies.Add( currency );
            }
        }

        public void Save()
        {
            var ctx = myProjectHost.Project.GetAssetsContext();
            ctx.SaveChanges();
        }
    }
}
