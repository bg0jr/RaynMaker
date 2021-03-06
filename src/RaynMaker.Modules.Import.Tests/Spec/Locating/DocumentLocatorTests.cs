﻿using System;
using System.IO;
using NUnit.Framework;
using Plainion.Validation;
using RaynMaker.Modules.Import.Spec;
using RaynMaker.Modules.Import.Spec.v2.Locating;
using RaynMaker.SDK;

namespace RaynMaker.Modules.Import.Tests.Spec.Locating
{
    [TestFixture]
    public class DocumentLocatorTests
    {
        [Test]
        public void Fragments_Add_ValueAdded()
        {
            var locator = new DocumentLocator();

            locator.Fragments.Add( new Request( "http://test1.org") );

            Assert.That( locator.Fragments[ 0 ].UrlString, Is.EqualTo( "http://test1.org" ) );
        }

        [Test]
        public void Fragments_Add_ChangeIsNotified()
        {
            var locator = new DocumentLocator();
            var counter = new CollectionChangedCounter( locator.Fragments );

            locator.Fragments.Add( new Request( "http://test1.org" ) );

            Assert.That( counter.Count, Is.EqualTo( 1 ) );
        }

        [Test]
        public void Clone_WhenCalled_AllMembersAreCloned()
        {
            var locator = new DocumentLocator(
                new Request( "http://test1.org" ),
                new Response( "http://test2.org" ) );

            var clone = FigureDescriptorFactory.Clone( locator );

            Assert.That( clone.Fragments[ 0 ].UrlString, Is.EqualTo( "http://test1.org" ) );
            Assert.That( clone.Fragments[ 1 ].UrlString, Is.EqualTo( "http://test2.org" ) );
        }

        [Test]
        public void Clone_WhenCalled_CollectionsAreMutableAndObservable()
        {
            var locator = new DocumentLocator( new Request( "http://test1.org" ) );

            var clone = FigureDescriptorFactory.Clone( locator );

            var dump = SpecDumper.Dump( locator );

            var counter = new CollectionChangedCounter( clone.Fragments );
            clone.Fragments.Add( new Response( "http://test2.org" ) );
            Assert.That( counter.Count, Is.EqualTo( 1 ) );
        }

        [Test]
        public void Validate_IsValid_DoesNotThrows()
        {
            var locator = new DocumentLocator(
                new Request( "http://test1.org" ),
                new Response( "http://test2.org" ) );

            RecursiveValidator.Validate( locator );
        }
    }
}
