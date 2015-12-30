using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using Plainion;
using RaynMaker.Import.Spec;
using RaynMaker.Import.Spec.v2.Extraction;

namespace RaynMaker.Import
{
    public class FormatFactory
    {
        public static IFigureExtractionDescriptor Create( Type type )
        {
            if( type == typeof( PathSeriesExtractionDescriptor ) )
            {
                return CreatePathSeriesFormat();
            }

            if( type == typeof( PathCellExtractionDescriptor ) )
            {
                return CreatePathCellFormat();
            }

            throw new NotSupportedException( "Unknown format type: " + type.Name );
        }

        private static IFigureExtractionDescriptor CreatePathSeriesFormat()
        {
            var format = new PathSeriesExtractionDescriptor( string.Empty );

            format.ValueFormat = new FormatColumn( "value", typeof( double ), "000,000.0000" );
            format.TimeAxisFormat = new FormatColumn( "time", typeof( int ), "0000" );

            return format;
        }

        private static IFigureExtractionDescriptor CreatePathCellFormat()
        {
            var format = new PathCellExtractionDescriptor( string.Empty );
            format.Expand = CellDimension.None;

            format.ValueFormat = new FormatColumn( "value", typeof( double ), "000,000.0000" );

            // not supported
            //format.TimeAxisFormat = new FormatColumn( "time", typeof( int ), "0000" );
            format.TimeAxisFormat = null;

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
