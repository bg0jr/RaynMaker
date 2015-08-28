using System;
using NUnit.Framework;
using RaynMaker.Import;

namespace RaynMaker.Import.Tests
{
    [TestFixture]
    public class ProxyPoolTest
    {
        [Test]
        public void Create()
        {
            ProxyPool pool = new ProxyPool();

            Assert.IsTrue( pool.Proxies.Count > 0 );
        }

        [Test]
        public void ValidateProxyNone()
        {
            ProxyPool pool = new ProxyPool();

            pool.ValidateProxy( pool.Proxies[ 0 ], ProxyPoolOptions.None );
            // should be ok because validation disabled
            pool.ValidateProxy( new Uri( "http://heise.de" ), ProxyPoolOptions.None );
        }
    }
}
