using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using Plainion;
using Plainion.Serialization;

namespace RaynMaker.Modules.Import.Spec.v2.Extraction
{
    /// <summary>
    /// Describes how to get a value from a string
    /// </summary>
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec/v2", Name = "ValueFormat" )]
    public class ValueFormat : SerializableBindableBase
    {
        private Regex myExtractionPattern;
        private string myFormat;
        private Type myType;
        private bool myInMillions;

        public ValueFormat()
        {
        }

        public ValueFormat( Type type )
            : this( type, null )
        {
        }

        public ValueFormat( Type type, string format )
        {
            Type = type;
            Format = format;
        }

        /// <summary>
        /// The extraction patttern defines the part of the input string
        /// that shall be formated. 
        /// <remarks>
        /// Group "1" will be used for further processing
        /// </remarks>
        /// </summary>
        public Regex ExtractionPattern
        {
            get { return myExtractionPattern; }
            set { SetProperty( ref myExtractionPattern, value ); }
        }

        [DataMember( Name = "ExtractionPattern" )]
        private string SerializedExtractionPattern
        {
            get { return ExtractionPattern == null ? null : ExtractionPattern.ToString(); }
            set { ExtractionPattern = value == null ? null : new Regex( value ); }
        }

        /// <summary>
        /// Will always be trimmed.
        /// If the format is a regex we will only take group zero.
        /// </summary>
        [DataMember]
        public string Format
        {
            get { return myFormat; }
            set { SetProperty( ref myFormat, value != null ? value.Trim() : null ); }
        }

        [Required]
        public Type Type
        {
            get { return myType; }
            set { SetProperty( ref myType, value ); }
        }

        [DataMember( Name = "Type" )]
        private string SerializedType
        {
            get { return Type.AssemblyQualifiedName; }
            set { Type = Type.GetType( value ); }
        }

        [DataMember]
        public bool InMillions
        {
            get { return myInMillions; }
            set { SetProperty( ref myInMillions, value ); }
        }
        
        /// <summary>
        /// The value will always be trimmed first.
        /// </summary>
        public object Convert( string value )
        {
            try
            {
                value = value != null ? value.Trim() : null;
                if( string.IsNullOrEmpty( value ) )
                {
                    return null;
                }

                if( ExtractionPattern != null )
                {
                    var md = ExtractionPattern.Match( value );
                    if( md.Success )
                    {
                        value = md.Groups[ 1 ].Value;
                    }
                    else
                    {
                        return null;
                    }
                }

                if( string.IsNullOrEmpty( value ) )
                {
                    return null;
                }

                if( value == "-" )
                {
                    return null;
                }

                if( string.IsNullOrEmpty( Format ) )
                {
                    return value;
                }

                if( Type == typeof( DateTime ) )
                {
                    return DateTime.ParseExact( value.ToString(), Format, CultureInfo.InvariantCulture );
                }

                char decimalSep = Format.ToCharArray().LastOrDefault( c => !Char.IsDigit( c ) );

                if( Type == typeof( long ) )
                {
                    // all other non-digit numbers are treated as thousands separators now
                    value = TakeBefore( value, decimalSep ).RemoveAll( c => !Char.IsDigit( c ) );
                    return long.Parse( TakeBefore( value, decimalSep ) ) * ( InMillions ? 1000000 : 1 );
                }
                else if( Type == typeof( int ) )
                {
                    // all other non-digit numbers are treated as thousands separators now
                    value = TakeBefore( value, decimalSep ).RemoveAll( c => !Char.IsDigit( c ) );
                    return int.Parse( TakeBefore( value, decimalSep ) ) * ( InMillions ? 1000000 : 1 );
                }

                NumberFormatInfo nfi = new NumberFormatInfo();
                nfi.NumberGroupSeparator = Format.ToCharArray().First( c => !Char.IsDigit( c ) ).ToString();
                nfi.NumberDecimalSeparator = decimalSep.ToString();
                nfi.NumberDecimalDigits = Format.Length - Format.IndexOf( decimalSep ) - 1;

                if( Type == typeof( float ) )
                {
                    return float.Parse( value, nfi ) * ( InMillions ? 1000000 : 1 );
                }
                else if( Type == typeof( double ) )
                {
                    return double.Parse( value, nfi ) * ( InMillions ? 1000000 : 1 );
                }

                return value;
            }
            catch( FormatException ex )
            {
                ex.AddContext( "Format", Format );
                ex.AddContext( "Value", value );
                throw;
            }
        }

        private static string TakeBefore( string s, char separator )
        {
            int num = s.IndexOf( separator );
            if( num >= 0 )
            {
                return s.Substring( 0, num );
            }
            return s;
        }

        public bool TryConvert( string valueStr, out object value )
        {
            value = null;

            try
            {
                value = Convert( valueStr );
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
