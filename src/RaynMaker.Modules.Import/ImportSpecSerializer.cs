using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using Plainion.Validation;
using RaynMaker.Modules.Import.Spec;
using RaynMaker.Modules.Import.Spec.v2;

namespace RaynMaker.Modules.Import
{
    /// <summary>
    /// Implements serialization and deserialization of all types of import spec entities.
    /// </summary>
    public class ImportSpecSerializer
    {
        public ImportSpecSerializer()
        {
            EnableValidation = true;
        }

        public bool EnableValidation { get; set; }

        public DataSourcesSheet ReadCompatible( Stream stream )
        {
            var serializer = CreateSerializer( typeof( DataSourcesSheet ) );
            var sheet = ( DataSourcesSheet )serializer.ReadObject( stream );

            return SpecMigration.Migrate( sheet );
        }

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
            if( EnableValidation )
            {
                RecursiveValidator.Validate( obj );
            }

            var serializer = CreateSerializer( typeof( T ) );
            serializer.WriteObject( stream, obj );
        }

        public void Write<T>( XmlWriter writer, T obj )
        {
            if( EnableValidation )
            {
                RecursiveValidator.Validate( obj );
            }

            var serializer = CreateSerializer( typeof( T ) );
            serializer.WriteObject( writer, obj );
        }
    }
}
