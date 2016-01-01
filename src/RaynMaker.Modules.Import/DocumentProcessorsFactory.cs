﻿using System;
using RaynMaker.Modules.Import.Documents;
using RaynMaker.Modules.Import.Parsers.Html;
using RaynMaker.Modules.Import.Parsers.Text;
using RaynMaker.Modules.Import.Spec;
using RaynMaker.Modules.Import.Spec.v2.Extraction;
using RaynMaker.Modules.Import.WinForms;

namespace RaynMaker.Modules.Import
{
    public class DocumentProcessorsFactory
    {
        public static IDocumentBrowser CreateBrowser()
        {
            return new DocumentBrowser( CreateNavigator() );
        }

        private static CachingNavigator CreateNavigator()
        {
            var navigator = new CachingNavigator(
                new Navigator(),
                new DocumentCache() );
            return navigator;
        }

        public static IDocumentBrowser CreateBrowser( SafeWebBrowser webBrowser )
        {
            return new WinFormsDocumentBrowser( CreateNavigator(), webBrowser );
        }

        public static IDocumentParser CreateParser( IDocument document, IFigureDescriptor format )
        {
            var htmlDocument = document as IHtmlDocument;
            if( htmlDocument != null )
            {
                return new HtmlParser( htmlDocument, format );
            }

            var textDocument = document as TextDocument;
            if( textDocument != null )
            {
                return new TextParser( textDocument, format );
            }

            throw new NotSupportedException( "Unable to find parser for document type: " + document.GetType() );
        }
    }
}