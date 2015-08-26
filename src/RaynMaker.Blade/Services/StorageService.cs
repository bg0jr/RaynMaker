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
            DataSheet sheet;

            using( var reader = XmlReader.Create( stock.Company.XdbPath ) )
            {
                var settings = new DataContractSerializerSettings();
                settings.PreserveObjectReferences = true;

                var knownTypes = KnownDatums.AllExceptPrice.ToList();
                knownTypes.Add( typeof( Price ) );
                settings.KnownTypes = knownTypes;

                var serializer = new DataContractSerializer( typeof( DataSheet ), settings );
                sheet = ( DataSheet )serializer.ReadObject( reader );
            }

            MigrateDatumToEF( stock, sheet, typeof( Price ), d => stock.Prices.Add( ( Price )d ) );

            sheet.Data.Add( new DatumSeries( typeof( Price ), stock.Prices.ToArray() ) );

            return sheet;
        }

        private void MigrateDatumToEF( Stock stock, DataSheet sheet, Type datumType, Action<IDatum> InsertStatement )
        {
            var series = sheet.Data.SeriesOf( datumType );
            if( series == null )
            {
                return;
            }

            var ctx = myProjectHost.Project.GetAssetsContext();

            foreach( var datum in series )
            {
                var currencyDatum = datum as AbstractCurrencyDatum;
                if( currencyDatum != null )
                {
                    var sheetCurrency = currencyDatum.Currency;
                    // enforce update by next line
                    currencyDatum.Currency = null;
                    currencyDatum.Currency = ctx.Currencies.Single( c => c.Name == sheetCurrency.Name );
                }

                InsertStatement( datum );
            }

            ctx.SaveChanges();

            SaveDataSheet( stock, sheet );
            sheet.Data.Remove( series );
        }

        public void SaveDataSheet( Stock stock, DataSheet sheet )
        {
            RecursiveValidator.Validate( sheet );

            var ctx = myProjectHost.Project.GetAssetsContext();
            ctx.SaveChanges();

            var allSeries = new List<IDatumSeries>();
            foreach( var datum in new[] { typeof( Price ) } )
            {
                var series = sheet.Data.SeriesOf( datum );
                sheet.Data.Remove( series );
            }

            using( var writer = XmlWriter.Create( stock.Company.XdbPath ) )
            {
                var settings = new DataContractSerializerSettings();
                settings.PreserveObjectReferences = true;

                var knownTypes = KnownDatums.AllExceptPrice.ToList();
                knownTypes.Add( typeof( Price ) );
                settings.KnownTypes = knownTypes;

                var serializer = new DataContractSerializer( typeof( DataSheet ), settings );
                serializer.WriteObject( writer, sheet );
            }

            foreach( var series in allSeries )
            {
                sheet.Data.Add( series );
            }
        }
    }
}
