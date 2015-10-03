using NUnit.Framework;

namespace RaynMaker.Entities.Tests
{
    [TestFixture]
    class StockTests
    {
        [Test]
        public void Prices_AfterCtor_NotNull()
        {
            var stock = new Stock();

            Assert.That( stock.Prices, Is.Not.Null );
        }
    }
}
