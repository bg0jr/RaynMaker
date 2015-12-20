﻿using System;
using System.Runtime.Serialization;
using NUnit.Framework;
using RaynMaker.Import.Spec;

namespace RaynMaker.Import.Tests.Spec
{
    [TestFixture]
    public class AbstractSeriesFormatTests : TestBase
    {
        [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec", Name = "DummyFormat" )]
        private class DummyFormat : AbstractSeriesFormat
        {
            public DummyFormat()
                : base( "dummy" )
            {
            }
        }

        [Test]
        public void Clone_WhenCalled_AllMembersAreCloned()
        {
            var format = new DummyFormat();
            format.Expand = CellDimension.Row;
            format.SeriesNamePosition = 17;
            format.TimeAxisPosition = 23;
            format.ValueFormat = new FormatColumn( "value", typeof( double ), "0.00" );
            format.TimeAxisFormat = new FormatColumn( "time", typeof( DateTime ), "G" );
            format.Anchor = Anchor.ForCell( new AbsolutePositionLocator( 4 ), new AbsolutePositionLocator( 8 ) );

            var clone = FormatFactory.Clone( format );

            Assert.That( clone.Expand, Is.EqualTo( format.Expand ) );
            Assert.That( clone.SeriesNamePosition, Is.EqualTo( format.SeriesNamePosition ) );
            Assert.That( clone.TimeAxisPosition, Is.EqualTo( format.TimeAxisPosition ) );

            Assert.That( clone.ValueFormat.Name, Is.EqualTo( format.ValueFormat.Name ) );
            Assert.That( clone.ValueFormat.Type, Is.EqualTo( format.ValueFormat.Type ) );
            Assert.That( clone.ValueFormat.Format, Is.EqualTo( format.ValueFormat.Format ) );

            Assert.That( clone.TimeAxisFormat.Name, Is.EqualTo( format.TimeAxisFormat.Name ) );
            Assert.That( clone.TimeAxisFormat.Type, Is.EqualTo( format.TimeAxisFormat.Type ) );
            Assert.That( clone.TimeAxisFormat.Format, Is.EqualTo( format.TimeAxisFormat.Format ) );

            Assert.That( ( ( AbsolutePositionLocator )clone.Anchor.Row ).Position, Is.EqualTo( 4 ) );
            Assert.That( ( ( AbsolutePositionLocator )clone.Anchor.Column ).Position, Is.EqualTo( 8 ) );
        }
    }
}
