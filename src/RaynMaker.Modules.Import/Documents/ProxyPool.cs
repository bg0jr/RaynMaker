using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace RaynMaker.Modules.Import.Documents
{
    [Flags]
    public enum ProxyPoolOptions
    {
        None,
        IsAvailable,

        // all following options imply IsAvailable
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1704" )]
        UsesCaptchas,

        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1704" )]
        NotUsesCaptchas
    }

    // TODO: add feature to validate proxies before searching for one
    // TODO: store ProxyOptions in myProxies for faster search
    // TODO: check proxy performance and rate proxies (return fastest first), 
    //       allow users to specify proxies with response time <= X only
    public class ProxyPool
    {
        private readonly Random myRandom = null;
        private readonly IList<Uri> myProxies = null;
        private static readonly Uri myProxyTestUrl = new Uri( "http://www.google.de" );
        //private int myLastSelectedProxy = -1;

        public ProxyPool()
        {
            myRandom = new Random( (int)DateTime.Now.Ticks );

            myProxies = new List<Uri>( 43 );

            /*
            http://www.dmoz.org/Computers/Internet/Proxying_and_Filtering/Hosted_Proxy_Services/Free/Proxy_Lists/
            http://www.digitalcybersoft.com/ProxyList/fresh-proxy-list.shtml
             */

            myProxies.Add( new Uri( "http://166.70.91.56:8080" ) ); //United States 	
            myProxies.Add( new Uri( "http://85.185.95.131:3128" ) ); //Iran 	
            myProxies.Add( new Uri( "http://24.119.212.49:8080" ) ); //United States 	
            myProxies.Add( new Uri( "http://165.228.128.11:80" ) ); //Australia 	
            myProxies.Add( new Uri( "http://202.82.116.26:3128" ) ); //Hong Kong 	
            myProxies.Add( new Uri( "http://200.83.4.60:80" ) ); //Chile 	
            myProxies.Add( new Uri( "http://68.153.117.54:80" ) ); //United States 	
            myProxies.Add( new Uri( "http://62.193.205.210:3128" ) ); //France 	
            myProxies.Add( new Uri( "http://219.93.178.162:3128" ) ); //Malaysia 	
            myProxies.Add( new Uri( "http://202.108.119.227:80" ) ); //China 	
            myProxies.Add( new Uri( "http://201.6.121.78:3128" ) ); //Brazil 	
            myProxies.Add( new Uri( "http://202.144.118.50:3128" ) ); //India 	
            myProxies.Add( new Uri( "http://212.30.25.190:8000" ) ); //Great Britain (UK)) 	
            myProxies.Add( new Uri( "http://200.88.223.99:80" ) ); //Dominican Republic 	
            myProxies.Add( new Uri( "http://83.206.143.73:80" ) ); //France 	
            myProxies.Add( new Uri( "http://81.90.20.180:8080" ) ); //Iraq 	
            myProxies.Add( new Uri( "http://192.76.71.99:80" ) ); //United States 	
            myProxies.Add( new Uri( "http://202.105.182.17:80" ) ); //China 	
            myProxies.Add( new Uri( "http://202.105.182.16:80" ) ); //China 	
            myProxies.Add( new Uri( "http://203.130.33.67:8080" ) ); //China 	
            myProxies.Add( new Uri( "http://195.76.242.227:80" ) ); //Spain 	
            myProxies.Add( new Uri( "http://209.200.38.171:80" ) ); //United States 	
            myProxies.Add( new Uri( "http://200.101.66.98:3128" ) ); //Brazil 	
            myProxies.Add( new Uri( "http://82.114.70.98:8080" ) ); //Czechoslovakia 	
            myProxies.Add( new Uri( "http://122.153.71.194:3128" ) ); //
            myProxies.Add( new Uri( "http://202.105.182.13:80" ) ); //China 	
            myProxies.Add( new Uri( "http://219.99.180.168:8080" ) ); //Japan 	
            myProxies.Add( new Uri( "http://210.192.124.166:8080" ) ); //China 	
            myProxies.Add( new Uri( "http://159.148.58.74:3128" ) ); //Latvia 	
            myProxies.Add( new Uri( "http://201.17.163.38:3128" ) ); //Brazil 	
            myProxies.Add( new Uri( "http://68.153.118.157:80" ) ); //United States 	
            myProxies.Add( new Uri( "http://190.67.42.166:8080" ) ); //
            myProxies.Add( new Uri( "http://202.105.182.11:80" ) ); //China 	
            myProxies.Add( new Uri( "http://68.153.117.51:80" ) ); //United States 	
            myProxies.Add( new Uri( "http://201.17.147.37:3128" ) ); //Brazil 	
            myProxies.Add( new Uri( "http://165.228.131.10:3128" ) ); //Australia 	
            myProxies.Add( new Uri( "http://200.174.85.195:3128" ) ); //Brazil 	
            myProxies.Add( new Uri( "http://166.111.33.59:80" ) ); //China 	
            myProxies.Add( new Uri( "http://80.97.12.51:3128" ) ); //Romania 	
            myProxies.Add( new Uri( "http://203.106.52.102:3128" ) ); //Malaysia 	
            myProxies.Add( new Uri( "http://202.105.182.33:80" ) ); //China 	
            myProxies.Add( new Uri( "http://196.36.198.91:80" ) ); //South Africa 	
            myProxies.Add( new Uri( "http://216.35.67.3:8080" ) ); //United States 	
        }

        public IList<Uri> Proxies
        {
            get { return myProxies; }
        }

        /// <summary>
        /// If options are None then the selected proxy is not validated.
        /// </summary>
        public Uri GetRandomProxy( ProxyPoolOptions options )
        {
            // create a list of proxies we can work with
            IList<Uri> workList = new List<Uri>( myProxies );

            int proxyIdx = -1;
            Uri proxy = null;
            while ( workList.Count > 0 )
            {
                // select a valid index
                proxyIdx = myRandom.Next() % workList.Count;

                proxy = workList[ proxyIdx ];
                workList.RemoveAt( proxyIdx );

                if ( options == ProxyPoolOptions.None )
                {
                    return proxy;
                }
                else if ( options == ValidateProxy( proxy, options ) )
                {
                    return proxy;
                }
            }

            return proxy;
        }

        /// <summary>
        /// Checks the proxy related to the specified options.
        /// Set the option you want to check for. The result contains the 
        /// options "enabled" on the proxy.
        /// A feature which is not specified is not checked.
        /// In case of an exception ProxyOptions.None is returned
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes" )]
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1822:MarkMembersAsStatic" )]
        public ProxyPoolOptions ValidateProxy( Uri proxy, ProxyPoolOptions options )
        {
            if ( options == ProxyPoolOptions.None )
            {
                return ProxyPoolOptions.None;
            }

            ProxyPoolOptions retOptions = ProxyPoolOptions.None;

            HttpWebResponse httpRes = null;

            try
            {
                HttpWebRequest httpReq = (HttpWebRequest)WebRequest.Create( myProxyTestUrl );
                httpReq.AllowAutoRedirect = false;

                WebProxy webProxy = new WebProxy( proxy );
                httpReq.Proxy = webProxy;

                httpRes = (HttpWebResponse)httpReq.GetResponse();

                // check availability
                // ProxyOptions.IsAvailable is implied
                if ( httpRes.StatusCode == HttpStatusCode.OK &&
                    httpRes.ResponseUri.ToString().StartsWith( httpReq.RequestUri.ToString(), StringComparison.OrdinalIgnoreCase ) )
                {
                    retOptions |= ProxyPoolOptions.IsAvailable;
                }
                else
                {
                    // no further tests possible
                    return retOptions;
                }

                // check for Captchas
                if ( (options & ProxyPoolOptions.UsesCaptchas) == ProxyPoolOptions.UsesCaptchas ||
                     (options & ProxyPoolOptions.NotUsesCaptchas) == ProxyPoolOptions.NotUsesCaptchas )
                {
                    Stream stream = httpRes.GetResponseStream();

                    string html = "";
                    using ( StreamReader reader = new StreamReader( stream, Encoding.UTF8 ) )
                    {
                        html = reader.ReadToEnd();
                    }

                    if ( html.IndexOf( "<title>Google</title>", StringComparison.OrdinalIgnoreCase ) >= 0 )
                    {
                        retOptions |= ProxyPoolOptions.NotUsesCaptchas;
                    }
                    else
                    {
                        retOptions |= ProxyPoolOptions.UsesCaptchas;
                    }
                }
            }
            catch ( Exception )
            {
                // TODO: log the exception
                retOptions = ProxyPoolOptions.None;
            }
            finally
            {
                if ( httpRes != null )
                {
                    httpRes.Close();
                }
            }

            return retOptions;
        }
    }
}
