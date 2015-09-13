using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using Plainion;
using RaynMaker.Entities;
using RaynMaker.Import.Core;
using RaynMaker.Import.Documents;
using RaynMaker.Import.Parsers.Html;
using RaynMaker.Import.Parsers.Html.WinForms;
using RaynMaker.Import.Spec;

namespace RaynMaker.Import.Web.ViewModels
{
    class BasicDatumProvider
    {
        private IBrowserBase myBrowser;

        public BasicDatumProvider( IBrowserBase browser )
        {
            myBrowser = browser;
        }

        public IHtmlDocument Document { get; private set; }

        public void Navigate( Site site, Stock stock )
        {
            Contract.RequiresNotNull( site, "site" );
            Contract.RequiresNotNull( stock, "stock" );

            var macroPattern = new Regex( @"(\$\{.*\})" );
            var filtered = new List<NavigatorUrl>();
            foreach( var navUrl in site.Navigation.Uris )
            {
                var md = macroPattern.Match( navUrl.UrlString );
                if( md.Success )
                {
                    var macro = md.Groups[ 1 ].Value;
                    var value = GetMacroValue( macro.Substring( 2, macro.Length - 3 ), stock );
                    if( value != null )
                    {
                        filtered.Add( new NavigatorUrl( navUrl.UrlType, navUrl.UrlString.Replace( macro, value ) ) );
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    filtered.Add( navUrl );
                }
            }

            Document = myBrowser.LoadDocument( filtered );
        }

        private string GetMacroValue( string macroId, Stock stock )
        {
            if( macroId.Equals( "isin", StringComparison.OrdinalIgnoreCase ) )
            {
                return stock.Isin;
            }

            throw new NotSupportedException( "Unknown macro: " + macroId );
        }

        public void Mark( PathSeriesFormat format )
        {
            Contract.RequiresNotNull( format, "format" );
            Contract.Invariant( Document != null, "Document not yet loaded" );

            var markupDoc = new MarkupDocument();
            markupDoc.Document = ( ( HtmlDocumentAdapter )Document ).Document;
            markupDoc.Anchor = format.Path;
            markupDoc.Dimension = format.Expand;
            markupDoc.SeriesName = format.SeriesName;
            if( format.Expand == CellDimension.Row )
            {
                markupDoc.ColumnHeaderRow = format.TimeAxisPosition;
                markupDoc.RowHeaderColumn = format.SeriesNamePosition;
            }
            else if( format.Expand == CellDimension.Column )
            {
                markupDoc.RowHeaderColumn = format.TimeAxisPosition;
                markupDoc.ColumnHeaderRow = format.SeriesNamePosition;
            }
            markupDoc.SkipColumns = format.SkipColumns;
            markupDoc.SkipRows = format.SkipRows;

            markupDoc.Apply();
        }

        public DataTable GetResult( PathSeriesFormat format )
        {
            Contract.RequiresNotNull( format, "format" );
            Contract.Invariant( Document != null, "Document not yet loaded" );

            var handle = new HtmlDocumentHandle( Document );
            var parser = DocumentParserFactory.CreateParser( handle, format );
            return parser.ExtractTable();
        }
    }
}
