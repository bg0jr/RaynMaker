using System;
using System.IO;
using Plainion;
using RaynMaker.Modules.Import.Spec.v2.Extraction;

namespace RaynMaker.Modules.Import
{
    public static class FigureDescriptorFactory
    {
        public static IFigureDescriptor Create( Type type )
        {
            if( type == typeof( PathSeriesDescriptor ) ) return CreatePathSeriesDescriptor();
            if( type == typeof( PathCellDescriptor ) ) return CreatePathCellDescriptor();
            if( type == typeof( PathSingleValueDescriptor ) ) return CreatePathSingleValueDescriptor();
            if( type == typeof( PathTableDescriptor ) ) return CreatePathTableDescriptor();
            if( type == typeof( CsvDescriptor ) ) return CreateCsvDescriptor();
            if( type == typeof( SeparatorSeriesDescriptor ) ) return CreateSeparatorSeriesDescriptor();

            throw new NotSupportedException( "Unknown descriptor type: " + type.Name );
        }

        private static IFigureDescriptor CreatePathSeriesDescriptor()
        {
            var descriptor = new PathSeriesDescriptor();

            descriptor.ValueFormat = new FormatColumn( "value", typeof( double ), "000,000.0000" );
            descriptor.TimeFormat = new FormatColumn( "time", typeof( int ), "0000" );

            return descriptor;
        }

        private static IFigureDescriptor CreatePathCellDescriptor()
        {
            var descriptor = new PathCellDescriptor();

            descriptor.ValueFormat = new FormatColumn( "value", typeof( double ), "000,000.0000" );

            return descriptor;
        }

        private static IFigureDescriptor CreatePathSingleValueDescriptor()
        {
            var descriptor = new PathSingleValueDescriptor();

            descriptor.ValueFormat = new FormatColumn( "value", typeof( double ), "000,000.0000" );

            return descriptor;
        }

        private static IFigureDescriptor CreatePathTableDescriptor()
        {
            return new PathTableDescriptor();
        }

        private static IFigureDescriptor CreateCsvDescriptor()
        {
            return new CsvDescriptor();
        }

        private static IFigureDescriptor CreateSeparatorSeriesDescriptor()
        {
            var desciptor = new SeparatorSeriesDescriptor();

            desciptor.ValueFormat = new FormatColumn( "value", typeof( double ), "000,000.0000" );
            desciptor.TimeFormat = new FormatColumn( "time", typeof( int ), "0000" );

            return desciptor;
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
    }
}
