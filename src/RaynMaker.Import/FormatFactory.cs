using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
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
            settings.DataContractResolver = new CloneDataContractResolver();

            var serializer = new DataContractSerializer( format.GetType(), settings );
            using( var stream = new MemoryStream() )
            {
                serializer.WriteObject( stream, format );
                stream.Seek( 0, SeekOrigin.Begin );
                return ( T )serializer.ReadObject( stream );
            }
        }

        private class CloneDataContractResolver : DataContractResolver
        {
            public override Type ResolveName( string typeName, string typeNamespace, Type declaredType, DataContractResolver knownTypeResolver )
            {
                if( typeNamespace == "https://github.com/bg0jr/RaynMaker/Import/Spec" && typeName == "ArrayOfNavigatorUrl" )
                {
                    return typeof( List<NavigationUrl> );
                }

                return knownTypeResolver.ResolveName( typeName, typeNamespace, declaredType, null );
            }

            public override bool TryResolveType( Type type, Type declaredType, DataContractResolver knownTypeResolver, out XmlDictionaryString typeName, out XmlDictionaryString typeNamespace )
            {
                return knownTypeResolver.TryResolveType( type, declaredType, null, out typeName, out typeNamespace );
            }
        }
    }
}
