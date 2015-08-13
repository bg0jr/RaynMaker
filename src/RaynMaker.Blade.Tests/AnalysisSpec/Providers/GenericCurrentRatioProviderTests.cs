using System;
using Moq;
using NUnit.Framework;
using RaynMaker.Blade.AnalysisSpec.Providers;
using RaynMaker.Blade.DataSheetSpec;
using RaynMaker.Blade.Engine;
using RaynMaker.Blade.Entities;

namespace RaynMaker.Blade.Tests.AnalysisSpec.Providers
{
    [TestFixture]
    public class GenericCurrentRatioProviderTests
    {
        [Test]
        public void ProvideValue_LhsSeriesEmpty_ReturnsMissingData()
        {
            var context = new Mock<IFigureProviderContext> { DefaultValue = DefaultValue.Mock };
            context.Setup( x => x.GetSeries( "S1" ) ).Returns( Series.Empty );
            context.Setup( x => x.GetSeries( "S2" ) ).Returns( new Series( CreateDatum( 2015, 1 ) ) );
            var provider = new GenericCurrentRatioProvider( "dummy", "S1", "S2", ( lhs, rhs ) => lhs + rhs );

            var result = provider.ProvideValue( context.Object );

            Assert.That( result, Is.InstanceOf<MissingData>() );
            Assert.That( ( ( MissingData )result ).Datum, Is.EqualTo( "S1" ) );
        }

        [Test]
        public void ProvideValue_RhsSeriesEmpty_ReturnsMissingData()
        {
            var context = new Mock<IFigureProviderContext> { DefaultValue = DefaultValue.Mock };
            context.Setup( x => x.GetSeries( "S1" ) ).Returns( new Series( CreateDatum( 2015, 1 ) ));
            context.Setup( x => x.GetSeries( "S2" ) ).Returns( Series.Empty );
            var provider = new GenericCurrentRatioProvider( "dummy", "S1", "S2", ( lhs, rhs ) => lhs + rhs );

            var result = provider.ProvideValue( context.Object );

            Assert.That( result, Is.InstanceOf<MissingData>() );
            Assert.That( ( ( MissingData )result ).Datum, Is.EqualTo( "S2" ) );
        }

        private IDatum CreateDatum( int year, double value )
        {
            return new Datum
            {
                Period = new YearPeriod( year ),
                Value = value,
                Source = "Dummy",
                Timestamp = DateTime.UtcNow
            };
        }
    }
}
