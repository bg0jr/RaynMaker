using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RaynMaker.Import.Spec;

namespace RaynMaker.Import.Parsers.Html
{
    public class HtmlForm
    {
        public HtmlForm( IHtmlElement element )
        {
            if( element == null )
            {
                throw new ArgumentNullException( "element" );
            }
            if( !element.TagName.Equals( "form", StringComparison.OrdinalIgnoreCase ) )
            {
                throw new ArgumentException( "Element is not a html form element" );
            }

            FormElement = element;
        }

        public IHtmlElement FormElement { get; private set; }

        public string Name
        {
            get
            {
                return FormElement.GetAttribute( "name" );
            }
        }

        public Uri CreateSubmitUrl()
        {
            return CreateSubmitUrl( Formular.Empty );
        }

        public Uri CreateSubmitUrl( Formular formular )
        {
            var builder = new UriBuilder();
            builder.Scheme = FormElement.Document.Url.Scheme;
            builder.Host = FormElement.Document.Url.Host;
            builder.Port = FormElement.Document.Url.Port;
            builder.Path = FormElement.GetAttribute( "action" );
            builder.Query = string.Join( "&", BuildParameters( formular ) );

            var urlString = builder.ToString();

            // we have to use obsolete API here to force Uri class to NOT encode our string - it is already encoded.
            // if we let Uri class do the default for a strange reason the Url gets reencoded - sometimes.
            return new Uri( urlString, true );
        }

        //<form action="/quote/historic/historic.csv" method="get" name="histcsv" style="margin:0px"> 
        //  <input type="hidden" name="secu" value="292"> 
        //  <input type="hidden" name="boerse_id" value="6">
        //  <input type="hidden" name="clean_split" value="1"> 
        //  <input type="hidden" name="clean_payout" value="0"> 
        //  <input type="hidden" name="clean_bezug" value="0"> <ul> <li><div class="fleft" style="width:45px">von:</div>
        //  <input name="min_time" value="25.2.2011" style="width:65px"></li> <li><div class="fleft" style="width:45px">bis:</div>
        //  <input name="max_time" value="25.2.2012" style="width:65px"></li> <li><div class="fleft" style="width:85px">Trennzeichen:</div>
        //  <input name="trenner" value=";" style="width:25px"></li> </ul> <div align="right" style="padding: 3px 3px 4px">
        //  <input name="go" value="Download" type="submit" style="font-size:12px;"></div> <div class="clearfloat"></div> 
        //</form> 
        private string[] BuildParameters( Formular formular )
        {
            var parameters = new Dictionary<string, string>();

            var inputElements = FormElement.GetInnerElements()
                .Where( child => child.TagName.Equals( "input", StringComparison.OrdinalIgnoreCase ) )
                .Where( child => child.GetAttribute( "type" ) != "submit" );
            foreach( var input in inputElements )
            {
                parameters[ input.GetAttribute( "name" ) ] = input.GetAttribute( "value" );
            }

            foreach( var param in formular.Parameters )
            {
                parameters[ param.Key ] = param.Value;
            }

            var urlPairs = parameters
                 .Select( entry => string.Format( "{0}={1}", HttpUtility.UrlEncode( entry.Key ), HttpUtility.UrlEncode( entry.Value ) ) )
                 .ToArray();

            return urlPairs;
        }
    }
}
