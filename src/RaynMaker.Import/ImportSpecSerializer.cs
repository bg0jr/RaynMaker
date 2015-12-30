using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using Plainion.Validation;
using RaynMaker.Import.Spec.v2;
using RaynMaker.Import.Spec.v2.Locating;

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

            return new DataContractSerializer( root, settings );
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
