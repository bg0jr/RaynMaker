using System;
using System.Linq;
using NUnit.Framework;
using RaynMaker.Entities.Datums;

namespace RaynMaker.Entities.UnitTests
{
    [TestFixture]
    class DynamicsTests
    {
        static object[] AllDatums = Dynamics.AllDatums.ToArray();

        [Test, TestCaseSource( "AllDatums" )]
        public void GetRelationship_ForDatum_NotNull( Type datumType )
        {
            var stock = new Stock { Company = new Company() };

            var refs = Dynamics.GetRelationship( stock, datumType );

            Assert.That( refs, Is.Not.Null );
        }

        [Test]
        public void GetRelationship_Price_NotNull()
        {
            var stock = new Stock { Company = new Company() };

            var refs = Dynamics.GetRelationship( stock, typeof( Price ) );

            Assert.That( refs, Is.Not.Null );
        }
    }
}
