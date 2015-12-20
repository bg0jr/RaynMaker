﻿using System.Runtime.Serialization;
using NUnit.Framework;
using RaynMaker.Import.Spec;

namespace RaynMaker.Import.Tests.Spec
{
    [TestFixture]
    public class AbstractTableFormatTests : TestBase
    {
        [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec", Name = "DummyFormat" )]
        private class DummyFormat : AbstractTableFormat
        {
            public DummyFormat( params FormatColumn[] columns )
                : base( "dummy", columns )
            {
            }
        }

        [Test]
        public void Clone_WhenCalled_AllMembersAreCloned()
        {
            var format = new DummyFormat(
                new FormatColumn( "c1", typeof( double ), "0.00" ),
                new FormatColumn( "c2", typeof( string ), "" ) );

            var clone = FormatFactory.Clone( format );

            Assert.That( clone.Columns[ 0 ].Name, Is.EqualTo( "c1" ) );
            Assert.That( clone.Columns[ 0 ].Type, Is.EqualTo( typeof( double ) ) );
            Assert.That( clone.Columns[ 0 ].Format, Is.EqualTo( "0.00" ) );

            Assert.That( clone.Columns[ 1 ].Name, Is.EqualTo( "c2" ) );
            Assert.That( clone.Columns[ 1 ].Type, Is.EqualTo( typeof( string ) ) );
            Assert.That( clone.Columns[ 1 ].Format, Is.EqualTo( "" ) );
        }
    }
}
