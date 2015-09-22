using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace RaynMaker.Infrastructure.Controls
{
    // http://blogs.msdn.com/b/prajakta/archive/2006/10/17/autp-detecting-hyperlinks-in-richtextbox-part-i.aspx
    // http://blogs.msdn.com/b/prajakta/archive/2006/11/28/auto-detecting-hyperlinks-in-richtextbox-part-ii.aspx
    public partial class NotesEditor : UserControl
    {
        public NotesEditor()
        {
            InitializeComponent();

            Loaded += OnLoaded;
        }

        private void OnLoaded( object sender, RoutedEventArgs e )
        {
            Document = myEditor.Document;
            Document.FontFamily = new FontFamily( "Arial" );
            Document.FontSize = 13;
        }

        public static readonly DependencyProperty DocumentProperty = DependencyProperty.Register( "Document",
            typeof( FlowDocument ), typeof( NotesEditor ), new PropertyMetadata( null ) );

        public FlowDocument Document
        {
            get { return ( FlowDocument )GetValue( DocumentProperty ); }
            set { SetValue( DocumentProperty, value ); }
        }

        private void OnSelectionChanged( object sender, RoutedEventArgs e )
        {
            object temp = myEditor.Selection.GetPropertyValue( Inline.FontWeightProperty );
            myBold.IsChecked = ( temp != DependencyProperty.UnsetValue ) && ( temp.Equals( FontWeights.Bold ) );

            temp = myEditor.Selection.GetPropertyValue( Inline.FontStyleProperty );
            myItalic.IsChecked = ( temp != DependencyProperty.UnsetValue ) && ( temp.Equals( FontStyles.Italic ) );

            temp = myEditor.Selection.GetPropertyValue( Inline.TextDecorationsProperty );
            myUnderline.IsChecked = ( temp != DependencyProperty.UnsetValue ) && ( temp.Equals( TextDecorations.Underline ) );
        }
    }
}
