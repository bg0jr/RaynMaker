﻿using System.Runtime.Serialization;
using NUnit.Framework;
using RaynMaker.Import.Spec.v2.Extraction;
using RaynMaker.Import.Tests;

namespace RaynMaker.Import.Tests.Spec.Extraction
{
    [TestFixture]
    public class CsvFormatTests 
    {
        [Test]
        public void Clone_WhenCalled_AllMembersAreCloned()
        {
            var format = new CsvDescriptor( "dummy", ";", 
                new FormatColumn( "c1", typeof( double ), "0.00" ),
                new FormatColumn( "c2", typeof( string ), "" ) );

            var clone = FormatFactory.Clone( format );

            Assert.That( clone.Separator, Is.EqualTo( ";" ) );

            Assert.That( clone.Columns[ 0 ].Name, Is.EqualTo( "c1" ) );
            Assert.That( clone.Columns[ 1 ].Name, Is.EqualTo( "c2" ) );
        }
    }
}
