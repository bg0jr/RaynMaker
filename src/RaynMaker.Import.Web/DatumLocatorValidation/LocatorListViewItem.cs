using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using Maui;
using RaynMaker.Import;
using RaynMaker.Import.Spec;

namespace RaynMaker.Import.Web.DatumLocatorValidation
{
    class LocatorListViewItem : ListViewItem
    {
        public class Data
        {
            public Data( ParameterizedDatumLocator datumLocator )
            {
                DatumLocator = datumLocator;
                ValidationResult = LocatorValidationResult.Empty;

                TypeDescriptor.AddAttributes( typeof( Anchor ), new TypeConverterAttribute( typeof( GenericTypeConverter ) ) );
                TypeDescriptor.AddAttributes( typeof( ICellLocator ), new TypeConverterAttribute( typeof( GenericTypeConverter ) ) );
            }

            [Browsable( false )]
            public ParameterizedDatumLocator DatumLocator
            {
                get;
                private set;
            }

            [CategoryAttribute( "Input" )]
            public string Datum
            {
                get { return DatumLocator.Datum; }
            }

            [CategoryAttribute( "Input" )]
            public string Isin
            {
                get { return GetParameter( "stock.isin" ); }
            }

            private string GetParameter( string key )
            {
                if ( DatumLocator.Parameters.ContainsKey( key ) )
                {
                    return DatumLocator.Parameters[ key ];
                }

                return string.Empty;
            }

            [Browsable( false )]
            public LocatorValidationResult ValidationResult
            {
                get;
                set;
            }

            [CategoryAttribute( "Output" )]
            public string[] Navigation
            {
                get { return ValidationResult.Navigation.Uris.Select( uri => uri.UrlString ).ToArray(); }
            }

            [CategoryAttribute( "Output" )]
            public string DocumentLocation
            {
                get { return ValidationResult.DocumentLocation; }
            }

            [CategoryAttribute( "Output" )]
            [TypeConverter( typeof( GenericTypeConverter ) )]
            public IFormat Format
            {
                get { return ValidationResult.Format; }
            }

            [CategoryAttribute( "Output" )]
            public string ErrorMessage
            {
                get { return ValidationResult.ErrorMessage; }
            }
        }

        public LocatorListViewItem( ParameterizedDatumLocator datumLocator )
            : base( datumLocator.Name )
        {
            Content = new Data( datumLocator );
        }

        public Data Content
        {
            get;
            private set;
        }
    }
}
