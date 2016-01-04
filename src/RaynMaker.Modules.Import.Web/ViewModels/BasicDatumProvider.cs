﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using Plainion;
using RaynMaker.Entities;
using RaynMaker.Modules.Import.Design;
using RaynMaker.Modules.Import.Documents;
using RaynMaker.Modules.Import.Documents.WinForms;
using RaynMaker.Modules.Import.Spec.v2;
using RaynMaker.Modules.Import.Spec.v2.Extraction;
using RaynMaker.Modules.Import.Spec.v2.Locating;

namespace RaynMaker.Modules.Import.Web.ViewModels
{
    class BasicDatumProvider
    {
        private IDocumentBrowser myBrowser;

        public BasicDatumProvider( IDocumentBrowser browser )
        {
            myBrowser = browser;
        }

        public IHtmlDocument Document { get; private set; }

        public void Navigate( DocumentType docType, DocumentLocator navigation, Stock stock )
        {
            Contract.RequiresNotNull( navigation, "navigation" );
            Contract.Requires( docType == DocumentType.Html, "Only DocumentType.Html supported" );
            Contract.RequiresNotNull( stock, "stock" );

            var macroPattern = new Regex( @"(\$\{.*\})" );
            var filtered = new List<DocumentLocationFragment>();
            foreach( var fragment in navigation.Fragments )
            {
                var md = macroPattern.Match( fragment.UrlString );
                if( md.Success )
                {
                    var macro = md.Groups[ 1 ].Value;
                    var value = GetMacroValue( macro.Substring( 2, macro.Length - 3 ), stock );
                    if( value != null )
                    {
                        filtered.Add( CreateFragment( fragment.GetType(), fragment.UrlString.Replace( macro, value ) ) );
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    filtered.Add( fragment );
                }
            }

            myBrowser.Navigate( docType, new DocumentLocator( filtered ) );
            Document = ( IHtmlDocument )myBrowser.Document;
        }

        private DocumentLocationFragment CreateFragment( Type type, string url )
        {
            if( type == typeof( Request ) )
            {
                return new Request( url );
            }
            else if( type == typeof( Response ) )
            {
                return new Response( url );
            }
            else
            {
                throw new NotSupportedException( "Unknown fragment type: " + type );
            }
        }

        private string GetMacroValue( string macroId, Stock stock )
        {
            if( macroId.Equals( "isin", StringComparison.OrdinalIgnoreCase ) )
            {
                return stock.Isin;
            }

            if( macroId.Equals( "Wpkn", StringComparison.OrdinalIgnoreCase ) )
            {
                return stock.Wpkn;
            }

            if( macroId.Equals( "Symbol", StringComparison.OrdinalIgnoreCase ) )
            {
                return stock.Symbol;
            }

            throw new NotSupportedException( "Unknown macro: " + macroId );
        }

        public void Mark( PathSeriesDescriptor format )
        {
            Contract.RequiresNotNull( format, "format" );
            Contract.Invariant( Document != null, "Document not yet loaded" );

            var markupBehavior = new HtmlMarkupBehavior();
            markupBehavior.Document = ( ( HtmlDocumentAdapter )Document ).Document;
            markupBehavior.PathToSelectedElement = format.Path;
            markupBehavior.Dimension = format.Orientation;
            //markupDoc.SeriesName = format.SeriesName;

            if( markupBehavior.Dimension == SeriesOrientation.Row )
            {
                markupBehavior.ColumnHeaderRow = format.TimesLocator.HeaderSeriesPosition;
                markupBehavior.RowHeaderColumn = format.ValuesLocator.HeaderSeriesPosition;

                markupBehavior.SkipColumns = null;
                markupBehavior.SkipRows = format.Excludes.ToArray();
            }
            else if( markupBehavior.Dimension == SeriesOrientation.Column )
            {
                markupBehavior.RowHeaderColumn = format.TimesLocator.HeaderSeriesPosition;
                markupBehavior.ColumnHeaderRow = format.ValuesLocator.HeaderSeriesPosition;

                markupBehavior.SkipColumns = format.Excludes.ToArray();
                markupBehavior.SkipRows = null;
            }

            markupBehavior.Apply();

            markupBehavior.Document = null;
        }

        public DataTable GetResult( IFigureDescriptor format )
        {
            Contract.RequiresNotNull( format, "format" );
            Contract.Invariant( Document != null, "Document not yet loaded" );

            var parser = DocumentProcessorsFactory.CreateParser( Document, format );
            return parser.ExtractTable();
        }
    }
}
