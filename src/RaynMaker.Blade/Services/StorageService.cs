using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.IO;
using System.Runtime.Serialization;
using System.Windows.Markup;
using System.Xml;
using Plainion.Validation;
using Plainion.Xaml;
using RaynMaker.Blade.Entities;
using RaynMaker.Blade.Entities.Datums;

namespace RaynMaker.Blade.Services
{
    [Export]
    class StorageService
    {
        public CurrenciesSheet LoadCurrencies( string path )
        {
            using( var reader = XmlReader.Create( path ) )
            {
                var serializer = new DataContractSerializer( typeof( CurrenciesSheet ) );
                return ( CurrenciesSheet )serializer.ReadObject( reader );
            }
        }

        public void SaveCurrencies( CurrenciesSheet sheet, string path )
        {
            RecursiveValidator.Validate( sheet );

            using( var writer = XmlWriter.Create( path ) )
            {
                var serializer = new DataContractSerializer( typeof( CurrenciesSheet ) );
                serializer.WriteObject( writer, sheet );
            }
        }

        public string LoadAnalysisTemplate( string path )
        {
            return File.ReadAllText( path );
        }

        public void SaveCurrencies( string template, string path )
        {
            File.WriteAllText( path, template );
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
