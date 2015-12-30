﻿using System.Runtime.Serialization;
using NUnit.Framework;
using RaynMaker.Import.Spec.v2.Extraction;
using RaynMaker.Import.Tests;

namespace RaynMaker.Import.Tests.Spec.Extraction
{
    [TestFixture]
    public class AbstractFormatTests : TestBase
    {
        [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec", Name = "DummyFormat" )]
        private class DummyFormat : AbstractFigureDescriptor
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
            format.Datum = "blue";
            format.InMillions = true;

            var clone = FormatFactory.Clone( format );

            Assert.That( clone.Datum, Is.EqualTo( format.Datum ) );
            Assert.That( clone.InMillions, Is.EqualTo( format.InMillions ) );
        }
    }
}
