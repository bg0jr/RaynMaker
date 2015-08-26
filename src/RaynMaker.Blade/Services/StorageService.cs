using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Linq;
using Plainion.Validation;
using Plainion.Xaml;
using RaynMaker.Blade.AnalysisSpec;
using RaynMaker.Blade.Entities;
using RaynMaker.Entities;
using RaynMaker.Entities.Datums;
using RaynMaker.Infrastructure;

namespace RaynMaker.Blade.Services
{
    [Export]
    class StorageService
    {
        private IProjectHost myProjectHost;

        [ImportingConstructor]
        public StorageService( IProjectHost projectHost )
        {
            myProjectHost = projectHost;
        }

        public CurrenciesSheet LoadCurrencies()
        {
            var ctx = myProjectHost.Project.GetAssetsContext();
            if( !ctx.Currencies.Any() )
            {
                ctx.Currencies.Add( new Currency { Name = "Euro" } );
                ctx.Currencies.Add( new Currency { Name = "Dollar" } );

                ctx.SaveChanges();
            }

            var dbSheet = new CurrenciesSheet();
            foreach( var currency in ctx.Currencies.Include( c => c.Translations ) )
            {
                dbSheet.Currencies.Add( currency );
            }
            return dbSheet;
        }

        public void SaveCurrencies( CurrenciesSheet sheet )
        {
            var ctx = myProjectHost.Project.GetAssetsContext();

            foreach( var currency in sheet.Currencies.Where( c => c.Id == 0 ) )
            {
                // TODO: ensure that Translation.Source is set - cleanup soon
                foreach( var translation in currency.Translations )
                {
                    translation.Source = currency;
                }

                ctx.Currencies.Add( currency );
            }

            ctx.SaveChanges();
        }

        public string LoadAnalysisTemplateText()
        {
            var ctx = myProjectHost.Project.GetAnalysisContext();
            if( !ctx.AnalysisTemplates.Any() )
            {
                var template = new RaynMaker.Entities.AnalysisTemplate();
                template.Name = "Default";

                using( var stream = GetType().Assembly.GetManifestResourceStream( "RaynMaker.Blade.Resources.Analysis.xaml" ) )
                {
                    using( var reader = new StreamReader( stream ) )
                    {
                        template.Template = reader.ReadToEnd();
                    }
                }
                ctx.AnalysisTemplates.Add( template );

                ctx.SaveChanges();
            }

            return ctx.AnalysisTemplates.Single().Template;
        }

        public RaynMaker.Blade.AnalysisSpec.AnalysisTemplate LoadAnalysisTemplate( CurrenciesSheet sheet )
        {
            var reader = new ValidatingXamlReader();

            var text = LoadAnalysisTemplateText();

            // required for currency translation during loading from text to entity
            CurrencyConverter.Sheet = sheet;

            return reader.Read<RaynMaker.Blade.AnalysisSpec.AnalysisTemplate>( XElement.Parse( text ) );
        }

        public void SaveAnalysisTemplate( string template )
        {
            var ctx = myProjectHost.Project.GetAnalysisContext();

            ctx.AnalysisTemplates.Single().Template = template;

            ctx.SaveChanges();
        }

        public DataSheet LoadDataSheet( Stock stock )
        {
            var sheet = new DataSheet();
            sheet.Data.Add( new DatumSeries( typeof( Price ), stock.Prices.ToArray() ) );
            sheet.Data.Add( new DatumSeries( typeof( Assets ), stock.Company.Assets.ToArray() ) );
            sheet.Data.Add( new DatumSeries( typeof( Debt ), stock.Company.Debts.ToArray() ) );
            sheet.Data.Add( new DatumSeries( typeof( Dividend ), stock.Company.Dividends.ToArray() ) );
            sheet.Data.Add( new DatumSeries( typeof( EBIT ), stock.Company.EBITs.ToArray() ) );
            sheet.Data.Add( new DatumSeries( typeof( Equity ), stock.Company.Equities.ToArray() ) );
            sheet.Data.Add( new DatumSeries( typeof( InterestExpense ), stock.Company.InterestExpenses.ToArray() ) );
            sheet.Data.Add( new DatumSeries( typeof( Liabilities ), stock.Company.Liabilities.ToArray() ) );
            sheet.Data.Add( new DatumSeries( typeof( NetIncome ), stock.Company.NetIncomes.ToArray() ) );
            sheet.Data.Add( new DatumSeries( typeof( Revenue ), stock.Company.Revenues.ToArray() ) );
            sheet.Data.Add( new DatumSeries( typeof( SharesOutstanding ), stock.Company.SharesOutstandings.ToArray() ) );

            return sheet;
        }

        public void SaveDataSheet( Stock stock, DataSheet sheet )
        {
            RecursiveValidator.Validate( sheet );

            var ctx = myProjectHost.Project.GetAssetsContext();

            Save( sheet, typeof( Price ), d => stock.Prices.Add( ( Price )d ) );
            Save( sheet, typeof( Assets ), d => stock.Company.Assets.Add( ( Assets )d ) );
            Save( sheet, typeof( Debt ), d => stock.Company.Debts.Add( ( Debt )d ) );
            Save( sheet, typeof( Dividend ), d => stock.Company.Dividends.Add( ( Dividend )d ) );
            Save( sheet, typeof( EBIT ), d => stock.Company.EBITs.Add( ( EBIT )d ) );
            Save( sheet, typeof( Equity ), d => stock.Company.Equities.Add( ( Equity )d ) );
            Save( sheet, typeof( InterestExpense ), d => stock.Company.InterestExpenses.Add( ( InterestExpense )d ) );
            Save( sheet, typeof( Liabilities ), d => stock.Company.Liabilities.Add( ( Liabilities )d ) );
            Save( sheet, typeof( NetIncome ), d => stock.Company.NetIncomes.Add( ( NetIncome )d ) );
            Save( sheet, typeof( Revenue ), d => stock.Company.Revenues.Add( ( Revenue )d ) );
            Save( sheet, typeof( SharesOutstanding ), d => stock.Company.SharesOutstandings.Add( ( SharesOutstanding )d ) );

            ctx.SaveChanges();
        }
        private void Save( DataSheet sheet, Type datumType, Action<IDatum> InsertStatement )
        {
            var series = sheet.Data.SeriesOf( datumType );
            if( series == null )
            {
                return;
            }

            foreach( AbstractDatum datum in series )
            {
                if( datum.Id == 0 )
                {
                    InsertStatement( datum );
                }
            }
        }
    }
}
