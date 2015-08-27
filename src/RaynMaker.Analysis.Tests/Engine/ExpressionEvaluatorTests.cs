using System;
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
        private FakeExpressionEvaluationContext myContext;
        private ExpressionEvaluator myEvaluator;

        [SetUp]
        public void SetUp()
        {
            myContext = new FakeExpressionEvaluationContext( new[] { 
                new FakeFigureProvider( "One", ctx => 1 ),
                new FakeFigureProvider( "STR", ctx => "Hello" ) });
            myEvaluator = new ExpressionEvaluator( myContext, typeof( FakeFunctions ) );
        }

        [Test]
        public void Evaluate_EmptyString_ReturnsNull()
        {
            Assert.That( myEvaluator.Evaluate( string.Empty ), Is.Null );
        }

        [Test]
        public void Evaluate_Number_ReturnsValueOfNumber()
        {
            Assert.That( myEvaluator.Evaluate( "42" ), Is.EqualTo( 42d ) );
        }

        [Test]
        public void Evaluate_DecimalNumber_ReturnsValueOfNumber()
        {
            Assert.That( myEvaluator.Evaluate( "4.2" ), Is.EqualTo( 4.2d ) );
        }

        [Test]
        public void Evaluate_SingleProvier_ReturnsNull()
        {
            Assert.That( myEvaluator.Evaluate( "One" ), Is.EqualTo( 1d ) );
        }

        [Test]
        public void Evaluate_ProviderWithProperty_ReturnsValueOfProviderProperty()
        {
            Assert.That( myEvaluator.Evaluate( "STR.Length" ), Is.EqualTo( 5 ) );
        }

        [Test]
        public void Evaluate_ProviderWithMethod_ReturnsValueOfProviderMethod()
        {
            Assert.That( myEvaluator.Evaluate( "STR.GetType().Name" ), Is.EqualTo( "String" ) );
        }

        [Test]
        public void Evaluate_FunctionWithoutArguments_ReturnsValueOfFunction()
        {
            Assert.That( myEvaluator.Evaluate( "PI()" ), Is.EqualTo( Math.PI ) );
        }

        [Test]
        public void Evaluate_FunctionWithOneArgument_ReturnsValueOfFunction()
        {
            Assert.That( myEvaluator.Evaluate( "Double(21)" ), Is.EqualTo( 42d ) );
        }

        [Test]
        public void Evaluate_FunctionWithTwoArguments_ReturnsValueOfFunction()
        {
            Assert.That( myEvaluator.Evaluate( "Add(21,21)" ), Is.EqualTo( 42d ) );
        }

        [Test]
        public void Evaluate_NestedFunctions_ReturnsValueOfFunction()
        {
            Assert.That( myEvaluator.Evaluate( "Double( Add( 21, 21 ) )" ), Is.EqualTo( 84 ) );
        }

        [Test]
        public void Evaluate_ProviderWithMethodInFunction_ReturnsValueOfFunctionFromValueOfProviderMethod()
        {
            Assert.That( myEvaluator.Evaluate( "Substring( STR.GetType().Name,3 )" ), Is.EqualTo( "Str" ) );
        }

        [Test]
        public void Evaluate_MemberCallOnResultOfFunction_ReturnsValueOfFunctionResultMember()
        {
            Assert.That( myEvaluator.Evaluate( "Add(21,21).GetType().Name" ), Is.EqualTo( "Double" ) );
        }
    }
}
