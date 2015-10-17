using System.Linq;
using NUnit.Framework;

namespace RaynMaker.Entities.Persistancy.Tests
{
    /// <summary>
    /// Combines all tests together which are relatest to low level Database setup - e.g. cascading delete constraints
    /// </summary>
    [TestFixture]
    class CascadingDeleteTests_Currency : DatabaseTestsBase
    {
        [Test]
        public void DeleteCurrency_WithTranslations_DeleteCascades()
        {
            var euro = new Currency { Name = "Euro" };
            var dollar = new Currency { Name = "Dollar" };
            var translation = new Translation
            {
                Source = euro,
                Target = dollar,
                Rate = 1
            };

            euro.Translations.Add( translation );

            using( var ctx = ( AssetsContext )Db.CreateAssetsContext() )
            {
                ctx.Currencies.Add( euro );
                ctx.Currencies.Add( dollar );

                ctx.SaveChangesSafe();

                DBAssert.RowExists( ctx.Database, "Currencies", "Id", euro.Id );
                DBAssert.RowExists( ctx.Database, "Currencies", "Id", dollar.Id );
                DBAssert.RowExists( ctx.Database, "Translations", "SourceId", euro.Id );
            }

            using( var ctx = ( AssetsContext )Db.CreateAssetsContext() )
            {
                euro = ctx.Currencies.Single( c => c.Name == "Euro" );
                dollar = ctx.Currencies.Single( c => c.Name == "Dollar" );
                var id = euro.Id;

                ctx.Currencies.Remove( euro );
                ctx.Currencies.Remove( dollar );

                ctx.SaveChangesSafe();

                DBAssert.RowNotExists( ctx.Database, "Currencies", "Id", euro.Id );
                DBAssert.RowNotExists( ctx.Database, "Currencies", "Id", dollar.Id );
                DBAssert.RowNotExists( ctx.Database, "Translations", "SourceId", euro.Id );
            }
        }
    }
}
