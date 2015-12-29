using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using Plainion.Validation;
using RaynMaker.Import.Spec;

namespace RaynMaker.Import
{
    /// <summary>
    /// Implements serialization and deserialization of all types of import spec entities.
    /// </summary>
    public class ImportSpecSerializer
    {
        public T Read<T>( Stream stream )
        {
            var serializer = CreateSerializer( typeof( T ) );
            return ( T )serializer.ReadObject( stream );
        }

        private static DataContractSerializer CreateSerializer( Type root )
        {
            var settings = new DataContractSerializerSettings();
            settings.KnownTypes = typeof( DataSource ).Assembly.GetTypes()
                .Where( t => !t.IsAbstract )
                .Where( t => t.GetCustomAttributes( false ).OfType<DataContractAttribute>().Any() )
                .ToList();
            settings.DataContractResolver = new CloneDataContractResolver();

            return new DataContractSerializer( root, settings );
        }

        private class CloneDataContractResolver : DataContractResolver
        {
            public override Type ResolveName( string typeName, string typeNamespace, Type declaredType, DataContractResolver knownTypeResolver )
            {
                Debug.WriteLine( typeName );
                if( typeNamespace == "https://github.com/bg0jr/RaynMaker/Import/Spec" && typeName == "ArrayOfNavigatorUrl" )
                {
                    return typeof( List<NavigationUrl> );
                }
                else if( typeNamespace == "https://github.com/bg0jr/RaynMaker/Import/Spec" && typeName == "NavigatorUrl" )
                {
                    return typeof( NavigationUrl );
                }

                return knownTypeResolver.ResolveName( typeName, typeNamespace, declaredType, null );
            }

            public override bool TryResolveType( Type type, Type declaredType, DataContractResolver knownTypeResolver, out XmlDictionaryString typeName, out XmlDictionaryString typeNamespace )
            {
                return knownTypeResolver.TryResolveType( type, declaredType, null, out typeName, out typeNamespace );
            }
        }

        public void Write<T>( Stream stream, T obj )
        {
            RecursiveValidator.Validate( obj );

            var serializer = CreateSerializer( typeof( T ) );
            serializer.WriteObject( stream, obj );
        }

        public void Write<T>( XmlWriter writer, T obj )
        {
            RecursiveValidator.Validate( obj );

            var serializer = CreateSerializer( typeof( T ) );
            serializer.WriteObject( writer, obj );
        }
    }
}
