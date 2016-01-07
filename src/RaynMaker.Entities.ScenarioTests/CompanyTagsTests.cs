using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace RaynMaker.Entities.Persistancy.Tests
{
    [TestFixture]
    class CompanyTagsTests : DatabaseTestsBase
    {
        [Test]
        public void Tags_Add_PersistedToDb()
        {
            var company = new Company { Name = "Dummy" };

            var tag = new Tag { Name = "Prio1" };
            company.Tags.Add( tag );

            using( var ctx = ( AssetsContext )Db.CreateAssetsContext() )
            {
                ctx.Companies.Add( company );

                ctx.SaveChangesSafe();

                DBAssert.RowExists( ctx.Database, "Tags", "Id", tag.Id );

                var result = ctx.Database.SqlQuery<CompanyTags>( string.Format( "SELECT * FROM CompanyTags WHERE Company_Id = {0}", company.Id ) );
                Assert.That( result, Is.Not.Empty );
                Assert.That( result.Single().Tag_Id, Is.EqualTo( tag.Id ) );
            }
        }

        [Test]
        public void Tags_RemoveRelationship_RelationShipRemovedTagRemains()
        {
            using( var ctx = ( AssetsContext )Db.CreateAssetsContext() )
            {
                var company = new Company { Name = "Dummy" };

                var tag = new Tag { Name = "Prio1" };
                company.Tags.Add( tag );

                ctx.Companies.Add( company );

                ctx.SaveChangesSafe();
            }

            using( var ctx = ( AssetsContext )Db.CreateAssetsContext() )
            {
                var tag = ctx.Tags.Single();
                var company = ctx.Companies.Single();
                company.Tags.Clear();

                ctx.SaveChangesSafe();

                DBAssert.RowExists( ctx.Database, "Tags", "Id", tag.Id );
                DBAssert.RowNotExists( ctx.Database, "CompanyTags", "Company_Id", company.Id );
            }
        }

        [Test]
        public void Tags_Remove_RelationShipRemoved()
        {
            using( var ctx = ( AssetsContext )Db.CreateAssetsContext() )
            {
                var company = new Company { Name = "Dummy" };

                var tag = new Tag { Name = "Prio1" };
                company.Tags.Add( tag );

                ctx.Companies.Add( company );

                ctx.SaveChangesSafe();
            }

            using( var ctx = ( AssetsContext )Db.CreateAssetsContext() )
            {
                var tag = ctx.Tags.Single();
                ctx.Tags.Remove( tag );

                ctx.SaveChangesSafe();

                DBAssert.RowNotExists( ctx.Database, "Tags", "Id", tag.Id );
                DBAssert.RowNotExists( ctx.Database, "CompanyTags", "Tag_Id", tag.Id );
            }
        }

        [Test]
        public void Company_Remove_RelationShipRemovedTagRemains()
        {
            using( var ctx = ( AssetsContext )Db.CreateAssetsContext() )
            {
                var company = new Company { Name = "Dummy" };

                var tag = new Tag { Name = "Prio1" };
                company.Tags.Add( tag );

                ctx.Companies.Add( company );

                ctx.SaveChangesSafe();
            }

            using( var ctx = ( AssetsContext )Db.CreateAssetsContext() )
            {
                var company = ctx.Companies.Single();
                var tag = ctx.Tags.Single();
                ctx.Companies.Remove( company );

                ctx.SaveChangesSafe();

                DBAssert.RowExists( ctx.Database, "Tags", "Id", tag.Id );
                DBAssert.RowNotExists( ctx.Database, "CompanyTags", "Company_Id", company.Id );
            }
        }

        private class CompanyTags
        {
            public long Tag_Id { get; private set; }
            public long Company_Id { get; private set; }
        }
    }
}
