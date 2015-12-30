using System;
using System.Runtime.Serialization;
using NUnit.Framework;
using RaynMaker.Import.Spec.v2.Extraction;
using RaynMaker.Import.Tests;

namespace RaynMaker.Import.Tests.Spec.Extraction
{
    [TestFixture]
    public class AbstractDimensionalFormatTest
    {
        [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec", Name = "DummyFormat" )]
        private class DummyFormat : AbstractDimensionalDescriptor
        {
            public DummyFormat()
                : base( "dummy" )
            {
            }
        }

        [Test]
        public void SkipRowsIsImmutable()
        {
            var format = new DummyFormat();

            var skipRows = new int[] { 1, 2, 3 };
            format.SkipRows = skipRows;

            skipRows[ 1 ] = 42;

            Assert.AreEqual( 1, format.SkipRows[ 0 ] );
            Assert.AreEqual( 2, format.SkipRows[ 1 ] );
            Assert.AreEqual( 3, format.SkipRows[ 2 ] );
        }

        [Test]
        public void SkipColumnsIsImmutable()
        {
            var format = new DummyFormat();

            var skipColumns = new int[] { 1, 2, 3 };
            format.SkipColumns = skipColumns;

            skipColumns[ 1 ] = 42;

            Assert.AreEqual( 1, format.SkipColumns[ 0 ] );
            Assert.AreEqual( 2, format.SkipColumns[ 1 ] );
            Assert.AreEqual( 3, format.SkipColumns[ 2 ] );
        }

        [Test]
        public void Clone_WhenCalled_AllMembersAreCloned()
        {
            var format = new DummyFormat();
            format.SkipRows = new[] { 5, 9 };
            format.SkipColumns = new[] { 11, 88 };

            var clone = FormatFactory.Clone( format );

            Assert.That( clone.SkipRows, Is.EquivalentTo( format.SkipRows ) );
            Assert.That( clone.SkipColumns, Is.EquivalentTo( format.SkipColumns ) );
        }
    }
}
