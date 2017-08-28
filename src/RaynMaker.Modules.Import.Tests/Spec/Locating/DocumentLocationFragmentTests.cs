using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Plainion.Validation;
using RaynMaker.Modules.Import.Spec.v2.Locating;

namespace RaynMaker.Modules.Import.UnitTests.Spec.Locating
{
    [TestFixture]
    class DocumentLocationFragmentTests
    {
        private class Dummy : DocumentLocationFragment
        {
            public Dummy( string url ) : base( url ) { }
            public Dummy( Uri url ) : base( url ) { }
        }

        [Test]
        public void Validate_IsValid_DoesNotThrows()
        {
            var locator = new Dummy( "http://www.me.com" );

            RecursiveValidator.Validate( locator );
        }

        [Test]
        public void Ctor_UrlIsNull_Throws()
        {
            var ex = Assert.Throws<ArgumentNullException>( () => new Dummy( ( Uri )null ) );
            Assert.That( ex.Message, Does.Contain( "Value cannot be null." + Environment.NewLine + "Parameter name: url" ) );
        }

        [Test]
        public void Ctor_UrlStringIsNullOrEmpty_Throws( [Values( null, "" )] string url )
        {
            var ex = Assert.Throws<ArgumentNullException>( () => new Dummy( url ) );
            Assert.That( ex.Message, Does.Contain( "string must not null or empty: url" ) );
        }
    }
}
