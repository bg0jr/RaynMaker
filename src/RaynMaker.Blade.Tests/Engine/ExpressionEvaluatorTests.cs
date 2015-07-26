using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using RaynMaker.Blade.Engine;
using RaynMaker.Blade.Tests.Engine.Fakes;

namespace RaynMaker.Blade.Tests.Engine
{
    [TestFixture]
    public class ExpressionEvaluatorTests
    {
        private IEnumerable<IFigureProvider> myProviders;
        private IFigureProviderContext myProviderContext;
        private ExpressionEvaluator myEvaluator;

        [SetUp]
        public void SetUp()
        {
            myProviders = new[] { 
                new FakeFigureProvider( "One", ctx => 1 ),
                new FakeFigureProvider( "STR", ctx => "Hello" ) };
            myProviderContext = new FakeFigureProviderContext();
            myEvaluator = new ExpressionEvaluator( myProviders, myProviderContext );
        }

        [Test]
        public void Evaluate_EmptyString_ReturnsNull()
        {
            Assert.That( myEvaluator.Evaluate( string.Empty ), Is.Null );
        }

        [Test]
        public void Evaluate_Number_ReturnsNull()
        {
            Assert.That( myEvaluator.Evaluate( "42" ), Is.EqualTo( 42d ) );
        }

        [Test]
        public void Evaluate_SingleProvier_ReturnsNull()
        {
            Assert.That( myEvaluator.Evaluate( "One" ), Is.EqualTo( 1d ) );
        }

        [Test]
        public void Evaluate_ProviderWithProperty_ReturnsTextWithValueOfProviderProperty()
        {
            Assert.That( myEvaluator.Evaluate( "STR.Length" ), Is.EqualTo( 5 ) );
        }

        [Test]
        public void Evaluate_ProviderWithMethod_ReturnsTextWithValueOfProviderMethod()
        {
            Assert.That( myEvaluator.Evaluate( "STR.GetType().Name" ), Is.EqualTo( "String" ) );
        }

        //Average(Last(ReturnOnEquity,5))
        [Test]
        public void Evaluate_SimpleFunctionCall_ReturnsValueOfFunction()
        {
        }
    }
}
