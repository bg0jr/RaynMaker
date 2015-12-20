using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using Plainion;
using RaynMaker.Import.Spec;

namespace RaynMaker.Import
{
    public class FormatFactory
    {
        public static IFormat Create( Type type )
        {
            if( type == typeof( PathSeriesFormat ) )
            {
                return CreatePathSeriesFormat();
            }

            if( type == typeof( PathCellFormat ) )
            {
                return CreatePathCellFormat();
            }

            throw new NotSupportedException( "Unknown format type: " + type.Name );
        }

        private static IFormat CreatePathSeriesFormat()
        {
            var format = new PathSeriesFormat( string.Empty );

            format.ValueFormat = new FormatColumn( "value", typeof( double ), "000,000.0000" );
            format.TimeAxisFormat = new FormatColumn( "time", typeof( int ), "0000" );

            return format;
        }

        private static IFormat CreatePathCellFormat()
        {
            var format = new PathCellFormat( string.Empty );
            format.Expand = CellDimension.None;

            format.ValueFormat = new FormatColumn( "value", typeof( double ), "000,000.0000" );

            // not supported
            //format.TimeAxisFormat = new FormatColumn( "time", typeof( int ), "0000" );
            format.TimeAxisFormat = null;

            return format;
        }

        public static T Clone<T>( T format )
        {
            Contract.RequiresNotNull( format, "format" );

            var settings = new DataContractSerializerSettings();
            settings.KnownTypes = typeof( IFormat ).Assembly.GetTypes()
                .Where( t => !t.IsAbstract )
                .Where( t => t.GetCustomAttributes( false ).OfType<DataContractAttribute>().Any() )
                .ToList();

            var serializer = new DataContractSerializer( format.GetType(), settings );
            using( var stream = new MemoryStream() )
            {
                serializer.WriteObject( stream, format );
                stream.Seek( 0, SeekOrigin.Begin );
                return ( T )serializer.ReadObject( stream );
            }
        }
    }
}
