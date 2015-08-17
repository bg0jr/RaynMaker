using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Navigation;
using RaynMaker.Blade.DataSheetSpec;
using RaynMaker.Blade.Engine;
using RaynMaker.Blade.Reporting;

namespace RaynMaker.Blade.AnalysisSpec
{
    public class ReferencesSection : IReportElement
    {
        public void Report( ReportContext context )
        {
            context.Document.Headline( "References" );

            var stock = context.Asset as Stock;
            if( stock == null )
            {
                var paragraph = new Paragraph();
                paragraph.Inlines.Add( new Run( "none" ) );
                context.Document.Blocks.Add( paragraph );

                return;
            }

            var list = new List();
            list.MarkerOffset = 10;
            list.MarkerStyle = TextMarkerStyle.Disc;

            if( stock.Overview.References.Count > 0 )
            {
                foreach( var financialRef in stock.Overview.References )
                {
                    var hyperlink = new Hyperlink( new Run( financialRef.Url.ToString() ) );
                    hyperlink.NavigateUri = financialRef.Url;
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
