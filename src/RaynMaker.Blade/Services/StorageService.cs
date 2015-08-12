using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using RaynMaker.Blade.DataSheetSpec;

namespace RaynMaker.Blade.Services
{
    [Export]
    class StorageService
    {
        [DataContract( Name = "CurrenciesSheet", Namespace = "https://github.com/bg0jr/RaynMaker" )]
        [KnownType( typeof( Currency ) )]
        private class SerializableCurrenciesSheet
        {
            public SerializableCurrenciesSheet( CurrenciesSheet sheet )
            {
                MaxAgeInDays = sheet.MaxAgeInDays;
                Currencies = new ObservableCollection<Currency>( sheet.Currencies );
            }

            [DataMember]
            public ObservableCollection<Currency> Currencies { get; private set; }

            [DataMember]
            public int MaxAgeInDays { get; set; }
        }

        public CurrenciesSheet LoadCurrencies( string path )
        {
            using( var reader = XmlReader.Create( path ) )
            {
                var serializer = new DataContractSerializer( typeof( SerializableCurrenciesSheet ) );
                var serializableSheet = ( SerializableCurrenciesSheet )serializer.ReadObject( reader );

                var sheet = new CurrenciesSheet();
                sheet.MaxAgeInDays = serializableSheet.MaxAgeInDays;
                foreach( var currency in serializableSheet.Currencies )
                {
                    sheet.Currencies.Add( currency );
                }

                return sheet;
            }
        }

        public void SaveCurrencies( CurrenciesSheet sheet, string path )
        {
            using( var writer = XmlWriter.Create( path ) )
            {
                var serializer = new DataContractSerializer( typeof( SerializableCurrenciesSheet ) );
                serializer.WriteObject( writer, new SerializableCurrenciesSheet( sheet ) );
            }
        }
    }
}
