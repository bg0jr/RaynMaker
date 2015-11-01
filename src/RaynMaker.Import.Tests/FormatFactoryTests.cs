using System;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using RaynMaker.Import.Spec;

namespace RaynMaker.Import.Tests
{
    [TestFixture]
    class FormatFactoryTests
    {
        static object[] AllFormats = typeof( IFormat ).Assembly.GetTypes()
            .Where( t => t.IsClass && !t.IsAbstract )
            .Where( t => t.GetInterfaces().Contains( typeof( IFormat ) ) )
            .ToArray();

        [Test, TestCaseSource( "AllFormats" )]
        public void Clone_WhenCalled_ClonesGivenFormat( Type formatType )
        {
            var obj = CreateFormat( formatType );

            var clone = FormatFactory.Clone( obj );

            Assert.That( obj, Is.Not.Null );
        }

        private IFormat CreateFormat( Type formatType )
        {
            var ctor = formatType.GetConstructors()
                .OrderBy( c => c.GetParameters().Length )
                .First();

            var args = ctor.GetParameters()
                .Select( paramInfo => CreateDefaultValue( paramInfo ) )
                .ToArray();
            return ( IFormat )Activator.CreateInstance( formatType, args );
        }

        private object CreateDefaultValue( ParameterInfo paramInfo )
        {
            if( paramInfo.ParameterType == typeof( string ) )
            {
                return "Dummy";
            }

            if( paramInfo.ParameterType.IsArray )
            {
                return Array.CreateInstance( paramInfo.ParameterType.GetElementType(), 0 );
            }

            throw new NotSupportedException( "Parameters of given type not supported: " + paramInfo.ParameterType );
        }
    }
}
