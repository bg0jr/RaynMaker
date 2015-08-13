using Moq;
using NUnit.Framework;
using RaynMaker.Blade.AnalysisSpec.Providers;
using RaynMaker.Blade.DataSheetSpec;
using RaynMaker.Blade.Engine;

namespace RaynMaker.Blade.Tests.AnalysisSpec.Providers
{
    [TestFixture]
    public class GenericCurrentRatioProviderTests
    {
        [Test]
        public void x()
        {
            var context = new Mock<IFigureProviderContext> { DefaultValue = DefaultValue.Mock };
            context.Setup( x => x.GetSeries( It.IsAny<string>() ) ).Returns( Series.Empty );
            var provider = new GenericCurrentRatioProvider( "dummy", "S1", "S2", ( lhs, rhs ) => lhs + rhs );
            
            var result = provider.ProvideValue( context.Object );

            Assert.That( result, Is.InstanceOf<MissingData>() );
        }
    }
}
