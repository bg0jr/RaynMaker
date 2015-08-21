using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using Plainion.Validation;
using RaynMaker.Blade.Entities;
using RaynMaker.Blade.Entities.Datums;
using RaynMaker.Infrastructure;
using System.Data.Entity;
using RaynMaker.Blade.AnalysisSpec;
using Plainion.Xaml;
using System.Xml.Linq;
using System.Diagnostics;

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

        public CurrenciesSheet LoadCurrencies( string path )
        {
            using( var reader = XmlReader.Create( path ) )
            {
                var settings = new DataContractSerializerSettings();
                settings.PreserveObjectReferences = true;

                var serializer = new DataContractSerializer( typeof( CurrenciesSheet ), settings );
                return ( CurrenciesSheet )serializer.ReadObject( reader );
            }
            
            //var ctx = myProjectHost.Project.GetCurrenciesContext();
            //if( !ctx.Currencies.Any() )
            //{
            //    using( var reader = XmlReader.Create( path ) )
            //    {
            //        var settings = new DataContractSerializerSettings();
            //        settings.PreserveObjectReferences = true;

            //        var serializer = new DataContractSerializer( typeof( CurrenciesSheet ), settings );
            //        var sheet = ( CurrenciesSheet )serializer.ReadObject( reader );
                    
            //        Debugger.Launch();

            //        foreach( var currency in sheet.Currencies )
            //        {
            //            ctx.Currencies.Add( currency );
            //        }
            //    }

            //    ctx.SaveChanges();
            //}

            //var dbSheet = new CurrenciesSheet();
            //foreach( var currency in ctx.Currencies )
            //{
            //    dbSheet.Currencies.Add( currency );
            //}
            //return dbSheet;
        }

        public void SaveCurrencies( CurrenciesSheet sheet, string path )
        {
            RecursiveValidator.Validate( sheet );

            using( var writer = XmlWriter.Create( path ) )
            {
                var settings = new DataContractSerializerSettings();
                settings.PreserveObjectReferences = true;

                var serializer = new DataContractSerializer( typeof( CurrenciesSheet ), settings );
                serializer.WriteObject( writer, sheet );
            }
        }

        public string LoadAnalysisTemplateText()
        {
            var ctx = myProjectHost.Project.GetAnalysisContext();
            if( !ctx.AnalysisTemplates.Any() )
            {
                var template = new RaynMaker.Entities.AnalysisTemplate();
                template.Name = "Default";

                using( var stream = GetType().Assembly.GetManifestResourceStream( "Analysis.xaml" ) )
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

        public AnalysisTemplate LoadAnalysisTemplate( CurrenciesSheet sheet )
        {
            var reader = new ValidatingXamlReader();

            var text = LoadAnalysisTemplateText();

            // required for currency translation during loading from text to entity
            CurrencyConverter.Sheet = sheet;

            return reader.Read<AnalysisTemplate>( XElement.Parse( text ) );
        }

        public void SaveAnalysisTemplate( string template )
        {
            var ctx = myProjectHost.Project.GetAnalysisContext();

            ctx.AnalysisTemplates.Single().Template = template;

            ctx.SaveChanges();
        }

        public DataSheet LoadDataSheet( string path )
        {
            using( var reader = XmlReader.Create( path ) )
            {
                var knownTypes = KnownDatums.AllExceptPrice.ToList();
                knownTypes.Add( typeof( Price ) );

                var serializer = new DataContractSerializer( typeof( DataSheet ), knownTypes );
                return ( DataSheet )serializer.ReadObject( reader );
            }
        }

        public void SaveDataSheet( DataSheet sheet, string path )
        {
            RecursiveValidator.Validate( sheet );

            using( var writer = XmlWriter.Create( path ) )
            {
                var knownTypes = KnownDatums.AllExceptPrice.ToList();
                knownTypes.Add( typeof( Price ) );

                var serializer = new DataContractSerializer( typeof( DataSheet ), knownTypes );
                serializer.WriteObject( writer, sheet );
            }
        }
    }
}
