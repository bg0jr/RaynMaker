﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using Plainion;
using RaynMaker.Entities;
using RaynMaker.Import.Documents;
using RaynMaker.Import.Parsers.Html;
using RaynMaker.Import.Parsers.Html.WinForms;
using RaynMaker.Import.Spec;
using RaynMaker.Import.Spec.v2.Extraction;
using RaynMaker.Import.Spec.v2.Locating;

namespace RaynMaker.Import.Web.ViewModels
{
    class BasicDatumProvider
    {
        private IDocumentBrowser myBrowser;

        public BasicDatumProvider( IDocumentBrowser browser )
        {
            myBrowser = browser;
        }

        public IHtmlDocument Document { get; private set; }

        public void Navigate( DocumentLocator navigation, Stock stock )
        {
            Contract.RequiresNotNull( navigation, "navigation" );
            Contract.Requires( navigation.DocumentType == DocumentType.Html, "Only DocumentType.Html supported" );
            Contract.RequiresNotNull( stock, "stock" );

            var macroPattern = new Regex( @"(\$\{.*\})" );
            var filtered = new List<LocatingFragment>();
            foreach( var navUrl in navigation.Uris )
            {
                var md = macroPattern.Match( navUrl.UrlString );
                if( md.Success )
                {
                    var macro = md.Groups[ 1 ].Value;
                    var value = GetMacroValue( macro.Substring( 2, macro.Length - 3 ), stock );
                    if( value != null )
                    {
                        filtered.Add( new LocatingFragment( navUrl.UrlType, navUrl.UrlString.Replace( macro, value ) ) );
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

            myBrowser.Navigate( new DocumentLocator( navigation.DocumentType, filtered ) );
            Document = ( ( HtmlDocumentHandle )myBrowser.Document ).Content;
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

        public void Mark( PathSeriesExtractionDescriptor format )
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

            markupDoc.Document = null;
        }

        public DataTable GetResult( IFigureExtractionDescriptor format )
        {
            Contract.RequiresNotNull( format, "format" );
            Contract.Invariant( Document != null, "Document not yet loaded" );

            var handle = new HtmlDocumentHandle( Document );
            var parser = DocumentProcessorsFactory.CreateParser( handle, format );
            return parser.ExtractTable();
        }
    }
}
