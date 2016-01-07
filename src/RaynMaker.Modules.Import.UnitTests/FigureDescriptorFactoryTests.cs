using System;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using RaynMaker.Modules.Import.Spec;
using RaynMaker.Modules.Import.Spec.v2.Extraction;

namespace RaynMaker.Modules.Import.UnitTests
{
    [TestFixture]
    class FigureDescriptorFactoryTests
    {
        static object[] AllFigureDescriptors = typeof( IFigureDescriptor ).Assembly.GetTypes()
            .Where( t => t.IsClass && !t.IsAbstract )
            .Where( t => t.GetInterfaces().Contains( typeof( IFigureDescriptor ) ) )
            .ToArray();

        [Test, TestCaseSource( "AllFigureDescriptors" )]
        public void Clone_WhenCalled_ClonesGivenDescriptor( Type descriptorType )
        {
            var obj = CreateDescriptor( descriptorType );

            var clone = FigureDescriptorFactory.Clone( obj );

            Assert.That( obj, Is.Not.Null );
        }

        private IFigureDescriptor CreateDescriptor( Type descriptorType )
        {
            var ctor = descriptorType.GetConstructors()
                .OrderBy( c => c.GetParameters().Length )
                .First();

            var args = ctor.GetParameters()
                .Select( paramInfo => CreateDefaultValue( paramInfo ) )
                .ToArray();
            return ( IFigureDescriptor )Activator.CreateInstance( descriptorType, args );
        }

        private object CreateDefaultValue( ParameterInfo paramInfo )
        {
            if( paramInfo.ParameterType == typeof( string ) )
            {
                return "Dummy";
            }

            if( paramInfo.ParameterType.IsArray )
            {
                var array = ( object[] )Array.CreateInstance( paramInfo.ParameterType.GetElementType(), 1 );
                if( paramInfo.ParameterType.GetElementType() == typeof( FormatColumn ) )
                {
                    array[ 0 ] = new FormatColumn( "c1", typeof( string ) );
                }
                else if( paramInfo.ParameterType.GetElementType() == typeof( ValueFormat ) )
                {
                    array[ 0 ] = new ValueFormat( typeof( string ) );
                }
                else
                {
                    array[ 0 ] = Activator.CreateInstance( paramInfo.ParameterType.GetElementType() );
                }
                return array;
            }

            throw new NotSupportedException( "Parameters of given type not supported: " + paramInfo.ParameterType );
        }
    }
}
