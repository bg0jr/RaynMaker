﻿using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Navigation;
using RaynMaker.Modules.Analysis.Engine;

namespace RaynMaker.Modules.Analysis.AnalysisSpec
{
    public class ReferencesSection : IReportElement
    {
        public void Report( ReportContext context )
        {
            context.Document.Headline( "References" );

            var list = new List();
            list.MarkerOffset = 10;
            list.MarkerStyle = TextMarkerStyle.Disc;

            if( context.Stock.Company.References.Count > 0 )
            {
                foreach( var financialRef in context.Stock.Company.References )
                {
                    var hyperlink = new Hyperlink( new Run( financialRef.Url.ToString() ) );
                    hyperlink.NavigateUri = new Uri( financialRef.Url );
                    hyperlink.RequestNavigate += OnRequestNavigate;

                    WeakEventManager<Hyperlink, RequestNavigateEventArgs>.AddHandler( hyperlink, "RequestNavigate", OnRequestNavigate );

                    list.ListItems.Add( new ListItem( new Paragraph( hyperlink ) ) );
                }
            }

            context.Document.Blocks.Add( list );
        }

        private void OnRequestNavigate( object sender, RequestNavigateEventArgs e )
        {
            Process.Start( e.Uri.AbsoluteUri );
            e.Handled = true;
        }
    }
}
