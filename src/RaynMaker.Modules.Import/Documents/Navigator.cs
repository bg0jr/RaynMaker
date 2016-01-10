using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Plainion;
using RaynMaker.Modules.Import.Spec;
using RaynMaker.Modules.Import.Spec.v2.Locating;

namespace RaynMaker.Modules.Import.Documents
{
    class Navigator : INavigator
    {
        public Uri Navigate( DocumentLocator locator, ILocatorMacroResolver macroResolver )
        {
            Contract.RequiresNotNull( locator, "locator" );
            Contract.RequiresNotNull( macroResolver, "macroResolver" );

            return NavigateToFinalSite( locator.Fragments, macroResolver );
        }

        /// <summary>
        /// Navigates to the final site specified by the user steps and
        /// returns the Uri of the last site.
        /// Regular expressions in response URLs (embedded in {}) are matched. The
        /// resulting parameter is set to the next request URL using 
        /// string.Format() at placeholder {0}.
        /// </summary>
        private Uri NavigateToFinalSite( IEnumerable<DocumentLocationFragment> fragments, ILocatorMacroResolver macroResolver )
        {
            string param = null;

            var lastFragment = fragments.Last();
            if( lastFragment is Response )
            {
                // last step is a response - take the one before
                lastFragment = fragments.ElementAt( fragments.Count() - 2 );
            }

            Uri currentUrl = null;
            foreach( var origFragment in fragments )
            {
                var fragment = macroResolver.Resolve( origFragment );

                Contract.Requires( !macroResolver.UnresolvedMacros.Any(), 
                    "Failed to resolve macros: {0}", string.Join( ",", macroResolver.UnresolvedMacros ) );

                if( fragment is Request )
                {
                    string url = fragment.UrlString;
                    if( param != null )
                    {
                        url = string.Format( url, param );
                    }
                    else if( HasPlaceHolder( url ) )
                    {
                        var ex = new ApplicationException( "Counldn't find a parameter for placeholder" );
                        ex.Data[ "Url" ] = url;

                        throw ex;
                    }

                    currentUrl = new Uri( url );

                    if( fragment == lastFragment )
                    {
                        // we can stop here - we created the url for the last step
                        // furster navigation not necessary
                        break;
                    }

                    currentUrl = SendRequest( currentUrl );
                }
                else if( fragment is Response )
                {
                    // get param from response url if any
                    param = PatternMatching.MatchEmbeddedRegex( fragment.UrlString, currentUrl.ToString() );
                }
                else if( fragment is SubmitFormular )
                {
                    currentUrl = SubmitFormular( currentUrl, ( ( SubmitFormular )fragment ).Formular );
                }
                else
                {
                    throw new NotSupportedException( "UrlType: " + fragment.GetType() );
                }
            }

            return currentUrl;
        }

        private bool HasPlaceHolder( string url )
        {
            int begin = url.IndexOf( '{' );
            int end = url.IndexOf( '}' );

            if( begin < 0 || end < 0 )
            {
                return false;
            }

            return Math.Abs( end - begin ) < 3;
        }

        private Uri SendRequest( Uri input )
        {
            WebResponse response = null;
            try
            {
                if( Navigating != null )
                {
                    Navigating( input );
                }

                var request = WebRequest.Create( input );
                response = request.GetResponse();

                return response.ResponseUri;
            }
            finally
            {
                if( response != null )
                {
                    response.Close();
                }
            }
        }

        /// <summary>
        /// 1. Get the document
        /// 2. find the formular specified by formular.Name (currently only Html supported)
        /// 3. fill the formular from the document with the additional/overwrite parameters
        /// 3. submit the formular
        /// </summary>
        private Uri SubmitFormular( Uri url, Formular formular )
        {
            throw new NotImplementedException( "SubmitFormular not implemented" );
            //using ( var loader = new WinFormHtmlDocumentLoader() )
            //{
            //    var document = loader.LoadHtmlDocument( url );
            //    var htmlForm = new Parsers.Html.WinForms.HtmlDocumentAdapter( document ).GetFormByName( formular.Name );
            //    if ( htmlForm == null )
            //    {
            //        return null;
            //    }

            //    var formSubmitUrl = htmlForm.CreateSubmitUrl( formular );

            //    return SendRequest( formSubmitUrl );
            //}
        }

        public event Action<Uri> Navigating;
    }
}
