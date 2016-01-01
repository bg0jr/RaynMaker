using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using Plainion;
using RaynMaker.Modules.Import.Spec;
using RaynMaker.Modules.Import.Spec.v2.Extraction;

namespace RaynMaker.Modules.Import
{
    public class FormatFactory
    {
        public static IFigureDescriptor Create( Type type )
        {
            if( type == typeof( PathSeriesDescriptor ) )
            {
                return CreatePathSeriesFormat();
            }

            if( type == typeof( PathCellDescriptor ) )
            {
                return CreatePathCellFormat();
            }

            throw new NotSupportedException( "Unknown format type: " + type.Name );
        }

        private static IFigureDescriptor CreatePathSeriesFormat()
        {
            var format = new PathSeriesDescriptor( string.Empty );

            format.ValueFormat = new FormatColumn( "value", typeof( double ), "000,000.0000" );
            format.TimeFormat = new FormatColumn( "time", typeof( int ), "0000" );

            return format;
        }

        private static IFigureDescriptor CreatePathCellFormat()
        {
            var format = new PathCellDescriptor( string.Empty );

            format.ValueFormat = new FormatColumn( "value", typeof( double ), "000,000.0000" );

            return format;
        }

        public static T Clone<T>( T obj )
        {
            Contract.RequiresNotNull( obj, "obj" );

            using( var stream = new MemoryStream() )
            {
                var serializer = new ImportSpecSerializer();
                serializer.Write( stream, obj );

                stream.Seek( 0, SeekOrigin.Begin );
                return serializer.Read<T>( stream );
            }
        }

        public static void Dump<T>( TextWriter writer, T obj )
        {
            Contract.RequiresNotNull( obj, "obj" );

            var settings = new XmlWriterSettings
            {
                Indent = true
            };

            using( var xmlWriter = XmlWriter.Create( writer, settings ) )
            {
                var serializer = new ImportSpecSerializer();
                serializer.Write( xmlWriter, obj );
            }
        }
    }
}
