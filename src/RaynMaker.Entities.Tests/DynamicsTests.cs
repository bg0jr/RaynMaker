using System;
using System.Linq;
using NUnit.Framework;
using RaynMaker.Entities.Figures;

namespace RaynMaker.Entities.Tests
{
    [TestFixture]
    class DynamicsTests
    {
        static object[] AllFigures = Dynamics.AllFigures.ToArray();

        [Test, TestCaseSource( "AllFigures" )]
        public void GetRelationship_ForFigure_NotNull( Type figureType )
        {
            var stock = new Stock { Company = new Company() };

            var refs = Dynamics.GetRelationship( stock, figureType );

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
