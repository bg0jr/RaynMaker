using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Data.Entity;
using System.Linq;
using Microsoft.Practices.Prism.Mvvm;
using Plainion;
using RaynMaker.Entities;
using RaynMaker.Infrastructure;

namespace RaynMaker.Blade.Model
{
    [Export]
    class Project : BindableBase
    {
        private IProjectHost myProjectHost;
        private int myMaxCurrencyTranslationsAgeInDays;

        [ImportingConstructor]
        public Project( IProjectHost projectHost )
        {
            myProjectHost = projectHost;
            myProjectHost.Changed += OnProjectChanged;

            Currencies = new ObservableCollection<Currency>();
            myMaxCurrencyTranslationsAgeInDays = -1;

            OnProjectChanged();
        }

        private void OnProjectChanged()
        {
            myMaxCurrencyTranslationsAgeInDays = -1;

            LoadCurrencies();
        }

        private void LoadCurrencies()
        {
            var ctx = myProjectHost.Project.GetAssetsContext();
            if( !ctx.Currencies.Any() )
            {
                ctx.Currencies.Add( new Currency { Name = "Euro" } );
                ctx.Currencies.Add( new Currency { Name = "Dollar" } );

                ctx.SaveChanges();
            }

            foreach( var currency in ctx.Currencies.Include( c => c.Translations ) )
            {
                Currencies.Add( currency );
            }

            OnPropertyChanged( () => Currencies );
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

        public void Save()
        {
            var ctx = myProjectHost.Project.GetAssetsContext();
            ctx.SaveChanges();
        }
    }
}
