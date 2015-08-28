using System.Windows;
using System.Windows.Documents;

namespace RaynMaker.Analysis.Engine
{
    static class FlowDocumentExtensions
    {
        public static Paragraph Headline( this FlowDocument self, string fmt, params object[] args )
        {
            var paragaph = new Paragraph( new Bold( new Run( string.Format( fmt, args ) ) ) );
            paragaph.FontSize = 15;
            paragaph.TextAlignment = TextAlignment.Center;

            self.Blocks.Add( paragaph );

            return paragaph;
        }

        public static Paragraph Paragraph( this FlowDocument self, string fmt, params object[] args )
        {
            var paragraph = new Paragraph( new Run( string.Format( fmt, args ) ) );
            self.Blocks.Add( paragraph );

            return paragraph;
        }

        public static TableCell Cell( this TableRow self, string fmt, params object[] args )
        {
            var cell = new TableCell( new Paragraph( new Run( string.Format( fmt, args ) ) ) );

            self.Cells.Add( cell );

            return cell;
        }
    }
}
