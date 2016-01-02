using System;
using System.IO;
using NUnit.Framework;
using Plainion.Validation;
using RaynMaker.Modules.Import.Spec;
using RaynMaker.Modules.Import.Spec.v2.Locating;

namespace RaynMaker.Modules.Import.UnitTests.Spec.Locating
{
    [TestFixture]
    public class DocumentLocatorTests
    {
        [Test]
        public void Clone_WhenCalled_AllMembersAreCloned()
        {
            var locator = new DocumentLocator(
                new Request( "http://test1.org" ),
                new Response( "http://test2.org" ) );

            var clone = FigureDescriptorFactory.Clone( locator );

            Assert.That( clone.FragmentsHashCode, Is.EqualTo( locator.FragmentsHashCode ) );

            Assert.That( clone.Fragments[ 0 ].UrlString, Is.EqualTo( "http://test1.org" ) );
            Assert.That( clone.Fragments[ 1 ].UrlString, Is.EqualTo( "http://test2.org" ) );
        }

        [Test]
        public void Validate_IsValid_DoesNotThrows()
        {
            var locator = new DocumentLocator(
                new Request( "http://test1.org" ),
                new Response( "http://test2.org" ) );

            RecursiveValidator.Validate( locator );
        }

        [Test]
        public void Ctor_MissingFragments_Throws()
        {
            var ex = Assert.Throws<ArgumentException>( () => new DocumentLocator() );

            Assert.That( ex.Message, Is.StringContaining( "Collection must not be empty: fragments" ) );
        }
    }
}
