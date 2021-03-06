﻿using System;
using System.Collections.Generic;
using Plainion;
using RaynMaker.Modules.Import.Documents;

namespace RaynMaker.Modules.Import.Parsers.Html
{
    /// <summary>
    /// Extensions to the Windows.Forms.HtmlElement class.
    /// </summary>
    public static class HtmlElementExtensions
    {
        /// <summary>
        /// Returns the root element of the document the HtmlElement belongs too.
        /// </summary>
        public static IHtmlElement GetRoot( this IHtmlElement child )
        {
            while( child != null && !child.TagName.Equals( "html", StringComparison.OrdinalIgnoreCase ) )
            {
                child = child.Parent;
            }

            return child;
        }

        /// <summary>
        /// Returns the <see cref="HtmlPath"/> of the HtmlElement up to root.
        /// </summary>
        public static HtmlPath GetPath( this IHtmlElement element )
        {
            Contract.RequiresNotNull( element, "element" );

            var fragments = new List<HtmlPathElement>();

            var cur = element;
            while( cur.Parent != null )
            {
                fragments.Add( new HtmlPathElement( cur.TagName, GetIndexOfChildWithinChildrenOfSameTag( cur ) ) );

                cur = cur.Parent;
            }

            fragments.Reverse();

            return new HtmlPath( fragments );
        }

        private static int GetIndexOfChildWithinChildrenOfSameTag( IHtmlElement element )
        {
            if( element.Parent == null )
            {
                // assume its valid HTML with <html/> as root element
                return 0;
            }

            int childPos = 0;
            foreach( var child in element.Parent.Children )
            {
                if( child.TagName.Equals( element.TagName, StringComparison.OrdinalIgnoreCase ) )
                {
                    if( child == element )
                    {
                        return childPos;
                    }
                    childPos++;
                }
            }

            throw new ArgumentException( "Could not find child pos for child: " + element.TagName );
        }
    }
}
