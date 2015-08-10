using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using RaynMaker.Blade.DataSheetSpec;
using RaynMaker.Blade.Engine;
using RaynMaker.Blade.Tests.Engine.Fakes;

namespace RaynMaker.Blade.Tests.Engine
{
    [TestFixture]
    public class FunctionCallTests
    {
        [Test]
        public void Ctor_FunctionIsNull_Throws()
        {
            Assert.Throws<ArgumentNullException>( () => new FunctionCall( null ) );
        }

        [Test]
        public void Invoke_TooLessArguments_Throws()
        {
            var function = new FunctionCall( typeof( FakeFunctions ).GetMethod( "Double" ) );

            var ex = Assert.Throws<InvalidOperationException>( () => function.Invoke() );
            Assert.That( ex.Message, Is.StringContaining( "parameter count mismatch" ) );
        }


        [Test]
        public void Invoke_IntPassedDoubleExpected_InvokeSucceeds()
        {
            var function = new FunctionCall( typeof( FakeFunctions ).GetMethod( "Double" ) );

            function.AddArgument( 21 );

            var result = function.Invoke();

            Assert.That( result, Is.EqualTo( 42 ) );
        }

        [Test]
        public void Invoke_DoublePassedIntExpected_InvokeSucceeds()
        {
            var function = new FunctionCall( typeof( FakeFunctions ).GetMethod( "Round" ) );

            function.AddArgument( 21.21d );
            function.AddArgument( 0d );

            var result = function.Invoke();

            Assert.That( result, Is.EqualTo( 21 ) );
        }

        [Test]
        public void AddArgument_TooMuchArguments_Throws()
        {
            var function = new FunctionCall( typeof( FakeFunctions ).GetMethod( "Double" ) );

            function.AddArgument( 1 );

            var ex = Assert.Throws<InvalidOperationException>( () => function.AddArgument( 2 ) );
            Assert.That( ex.Message, Is.StringContaining( "parameter count mismatch" ) );
        }

        [Test]
        public void AddArgument_IntPassedDoubleExpected_TypeConverted()
        {
            var function = new FunctionCall( typeof( FakeFunctions ).GetMethod( "Double" ) );

            function.AddArgument( 21 );

            Assert.That( function.Arguments.First(), Is.InstanceOf<double>() );
        }

        [Test]
        public void AddArgument_DoublePassedIntExpected_TypeConverted()
        {
            var function = new FunctionCall( typeof( FakeFunctions ).GetMethod( "Round" ) );

            function.AddArgument( 21.21d );
            function.AddArgument( 0d );

            Assert.That( function.Arguments.Last(), Is.InstanceOf<int>() );
        }

        [Test]
        public void AddArgument_ImplementedInterfaceExpected_ArgumentExpected()
        {
            var function = new FunctionCall( typeof( FakeFunctions ).GetMethod( "Count" ) );

            function.AddArgument( new Series() );

            Assert.That( function.Arguments.Last(), Is.InstanceOf<Series>() );
        }

        [Test]
        public void AddArgument_Null_Accepted()
        {
            var function = new FunctionCall( typeof( FakeFunctions ).GetMethod( "Count" ) );

            function.AddArgument( null );

            Assert.That( function.Arguments.Last(), Is.Null );
        }
    }
}
