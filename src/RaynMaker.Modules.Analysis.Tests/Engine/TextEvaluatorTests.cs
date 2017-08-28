using System.Collections.Generic;
using NUnit.Framework;
using RaynMaker.Modules.Analysis.Engine;
using RaynMaker.Modules.Analysis.Tests.Engine.Fakes;

namespace RaynMaker.Modules.Analysis.Tests.Engine
{
    [TestFixture]
    public class TextEvaluatorTests
    {
        private FakeExpressionEvaluationContext myContext;
        private TextEvaluator myEvaluator;

        [SetUp]
        public void SetUp()
        {
            myContext = new FakeExpressionEvaluationContext( new[] { 
                new FakeFigureProvider( "One", ctx => 1d ), 
                new FakeFigureProvider( "Null", ctx => null ), 
                new FakeFigureProvider( "STR", ctx => "Hello" ) } );
            myEvaluator = new TextEvaluator( new ExpressionEvaluator( myContext ) );
        }

        [Test]
        public void Evaluate_EmptyString_ReturnsEmptyString()
        {
            Assert.That( myEvaluator.Evaluate( string.Empty ), Is.Empty );
        }

        [Test]
        public void Evaluate_PlainText_ReturnsSameText()
        {
            Assert.That( myEvaluator.Evaluate( "Sample Text" ), Is.EqualTo( "Sample Text" ) );
        }

        [Test]
        [SetCulture( "en-US" )]
        public void Evaluate_TextWithProviders_ReturnsTextWithProviderValue()
        {
            Assert.That( myEvaluator.Evaluate( "Value: ${STR} ${One}" ), Is.EqualTo( "Value: Hello 1.00" ) );
        }

        [Test]
        public void Evaluate_ProviderReturnsNull_ReturnsNA()
        {
            Assert.That( myEvaluator.Evaluate( "${Null}" ), Is.EqualTo( "n.a." ) );
        }

        [Test]
        public void Evaluate_WhenCalled_ReturnsFormattedValueOfProvider()
        {
            Assert.That( myEvaluator.Evaluate( "${One}" ), Is.EqualTo( ( 1d ).ToString( "0.00" ) ) );
        }

        [Test]
        public void ProvideValue_WhenCalled_ReturnsValueOfProvider()
        {
            Assert.That( myEvaluator.ProvideValue( "${One}" ), Is.EqualTo( 1d ) );
        }
    }
}
