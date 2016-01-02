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
    public class FigureDescriptorFactory
    {
        public static IFigureDescriptor Create( Type type )
        {
            if( type == typeof( PathSeriesDescriptor ) )
            {
                return CreatePathSeriesDescriptor();
            }

            if( type == typeof( PathCellDescriptor ) )
            {
                return CreatePathCellDescriptor();
            }

            throw new NotSupportedException( "Unknown format type: " + type.Name );
        }

        private static IFigureDescriptor CreatePathSeriesDescriptor()
        {
            var desciptor = new PathSeriesDescriptor( string.Empty );

            desciptor.ValueFormat = new FormatColumn( "value", typeof( double ), "000,000.0000" );
            desciptor.TimeFormat = new FormatColumn( "time", typeof( int ), "0000" );

            return desciptor;
        }

        private static IFigureDescriptor CreatePathCellDescriptor()
        {
            var descriptor = new PathCellDescriptor( string.Empty );

            descriptor.ValueFormat = new FormatColumn( "value", typeof( double ), "000,000.0000" );

            return descriptor;
        }

        public static T Clone<T>( T obj )
        {
            Contract.RequiresNotNull( obj, "obj" );

            using( var stream = new MemoryStream() )
            {
                var serializer = new ImportSpecSerializer { EnableValidation = false };
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
                var serializer = new ImportSpecSerializer { EnableValidation = false };
                serializer.Write( xmlWriter, obj );
            }
        }
    }
}
