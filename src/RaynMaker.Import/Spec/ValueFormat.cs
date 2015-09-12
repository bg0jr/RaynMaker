using System;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using Plainion;

namespace RaynMaker.Import.Spec
{
    /// <summary>
    /// Describes how to get a value from a string
    /// </summary>
    [Serializable]
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec", Name = "ValueFormat" )]
    public class ValueFormat : IEquatable<ValueFormat>
    {
        private string myFormat = null;

        /// <summary>
        /// Describes a string with no special format.
        /// </summary>
        public ValueFormat()
            : this( typeof( string ), null )
        {
        }

        /// <summary>
        /// Describes a string with no special format.
        /// </summary>
        public ValueFormat( Regex extractionPattern )
            : this( typeof( string ), null, extractionPattern )
        {
        }

        /// <summary>
        /// Describes a value of the given type with no special format.
        /// </summary>
        public ValueFormat( Type type )
            : this( type, null )
        {
        }

        /// <summary>
        /// Describes a value of the given type and format.
        /// </summary>
        public ValueFormat( Type type, string format )
            : this( type, format, null )
        {
        }

        /// <summary>
        /// Describes a value of the given type and format with extraction pattern.
        /// </summary>
        public ValueFormat( Type type, string format, Regex extractionPattern )
        {
            ExtractionPattern = extractionPattern;
            Format = format;
            Type = ( type != null ? type : typeof( string ) );
        }

        /// <summary/>
        public ValueFormat( ValueFormat other )
        {
            Contract.RequiresNotNull( other, "other" );

            Format = other.Format;
            Type = other.Type;
            ExtractionPattern = other.ExtractionPattern;
        }

        /// <summary>
        /// The extraction patttern defines the part of the input string
        /// that shall be formated. 
        /// <remarks>
        /// Group "1" will be used for further processing
        /// </remarks>
        /// </summary>
        [DataMember]
        public Regex ExtractionPattern { get; private set; }

        /// <summary>
        /// Will always be trimmed.
        /// If the format is a regex we will only take group zero.
        /// </summary>
        [DataMember]
        public string Format
        {
            get { return myFormat; }
            set { myFormat = value != null ? value.Trim() : null; }
        }

        /// <summary/>
        public Type Type { get; private set; }

        [DataMember( Name = "Type" )]
        private string SerializedType
        {
            get { return Type.AssemblyQualifiedName; }
            set { Type = Type.GetType( value ); }
        }

        /// <summary/>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append( "Type = " );
            sb.Append( Type.FullName );
            sb.Append( ", Format = " );
            sb.Append( Format );
            sb.Append( ", ExtractionPattern = " );
            sb.Append( ExtractionPattern );

            return sb.ToString();
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
                    return long.Parse( TakeBefore( value, decimalSep ) );
                }
                else if( Type == typeof( int ) )
                {
                    // all other non-digit numbers are treated as thousands separators now
                    value = TakeBefore( value, decimalSep ).RemoveAll( c => !Char.IsDigit( c ) );
                    return int.Parse( TakeBefore( value, decimalSep ) );
                }

                NumberFormatInfo nfi = new NumberFormatInfo();
                nfi.NumberGroupSeparator = Format.ToCharArray().First( c => !Char.IsDigit( c ) ).ToString();
                nfi.NumberDecimalSeparator = decimalSep.ToString();
                nfi.NumberDecimalDigits = Format.Length - Format.IndexOf( decimalSep ) - 1;

                if( Type == typeof( float ) )
                {
                    return float.Parse( value, nfi );
                }
                else if( Type == typeof( double ) )
                {
                    return double.Parse( value, nfi );
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

        public bool Equals( ValueFormat other )
        {
            return GetHashCode() == other.GetHashCode();
        }

        public override bool Equals( object other )
        {
            if( !( other is ValueFormat ) )
            {
                return false;
            }

            return Equals( ( ValueFormat )other );
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }
}
