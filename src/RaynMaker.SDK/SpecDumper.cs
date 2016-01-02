using System.IO;
using System.Xml;
using Plainion;
using RaynMaker.Modules.Import;

namespace RaynMaker.SDK
{
    public static class SpecDumper
    {
        public static string Dump<T>( T obj )
        {
            using( var writer = new StringWriter() )
            {
                Dump( obj, writer );
                return writer.ToString();
            }
        }

        public static void Dump<T>( T obj, TextWriter writer )
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
