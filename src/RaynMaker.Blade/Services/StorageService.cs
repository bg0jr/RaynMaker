﻿using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Runtime.Serialization;
using System.Windows.Markup;
using System.Xml;
using Plainion.Validation;
using Plainion.Xaml;
using RaynMaker.Blade.DataSheetSpec;
using RaynMaker.Blade.Entities;

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
            RecursiveValidator.Validate( sheet );

            using( var writer = XmlWriter.Create( path ) )
            {
                var serializer = new DataContractSerializer( typeof( SerializableCurrenciesSheet ) );
                serializer.WriteObject( writer, new SerializableCurrenciesSheet( sheet ) );
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
            var reader = new ValidatingXamlReader();

            return reader.Read<DataSheet>( path );
        }

        public void SaveDataSheet( DataSheet Sheet, string path )
        {
            throw new System.NotImplementedException();
        }
    }
}