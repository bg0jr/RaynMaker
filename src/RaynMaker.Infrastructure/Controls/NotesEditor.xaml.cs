using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace RaynMaker.Infrastructure.Controls
{
    public partial class NotesEditor : UserControl
    {
        public NotesEditor()
        {
            InitializeComponent();

            myFontFamily.ItemsSource = Fonts.SystemFontFamilies.OrderBy( f => f.Source );
            myFontSize.ItemsSource = new List<double>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };

            myEditor.PreviewKeyDown += OnPreviewKeyDown;
            Loaded += OnLoaded;
        }

        private void OnLoaded( object sender, RoutedEventArgs e )
        {
            Document = myEditor.Document;
        }

        public static readonly DependencyProperty DocumentProperty = DependencyProperty.Register( "Document",
            typeof( FlowDocument ), typeof( NotesEditor ), new PropertyMetadata( null ) );

        public FlowDocument Document
        {
            get { return ( FlowDocument )GetValue( DocumentProperty ); }
            set { SetValue( DocumentProperty, value ); }
        }

        private void OnPreviewKeyDown( object sender, KeyEventArgs e )
        {
            if( e.Key == Key.Back )
            {
                var start = myEditor.CaretPosition;
                var t = start.GetTextInRun( LogicalDirection.Backward );
                var end = start.GetNextContextPosition( LogicalDirection.Backward );
                var t1 = end.GetTextInRun( LogicalDirection.Backward );
                //myEditor.Selection.Select( start, end );
                //myEditor.Selection.ApplyPropertyValue( TextElement.ForegroundProperty, Brushes.Black );
                //myEditor.Selection.Select( start, start );
                //e.Handled = true;
            }

            //TextRange tr = new TextRange( mainRTB.Selection.Start, mainRTB.Selection.End );
            //Hyperlink hlink = new Hyperlink( tr.Start, tr.End );
            //hlink.NavigateUri = new Uri( "http://www.google.com/" );
        }

        // https://social.msdn.microsoft.com/Forums/vstudio/en-US/29340547-0fe6-42f9-8350-aa24ffc7d67e/richtextbox-insert-hyperlink-at-caret-position?forum=wpf
        // http://cxhelloworld.blogspot.de/2012/05/hyperlink-in-wpf-richtextbox.html

        private void OnSelectionChanged( object sender, RoutedEventArgs e )
        {
            object temp = myEditor.Selection.GetPropertyValue( Inline.FontWeightProperty );
            myBold.IsChecked = ( temp != DependencyProperty.UnsetValue ) && ( temp.Equals( FontWeights.Bold ) );

            temp = myEditor.Selection.GetPropertyValue( Inline.FontStyleProperty );
            myItalic.IsChecked = ( temp != DependencyProperty.UnsetValue ) && ( temp.Equals( FontStyles.Italic ) );

            temp = myEditor.Selection.GetPropertyValue( Inline.TextDecorationsProperty );
            myUnderline.IsChecked = ( temp != DependencyProperty.UnsetValue ) && ( temp.Equals( TextDecorations.Underline ) );

            temp = myEditor.Selection.GetPropertyValue( Inline.FontFamilyProperty );
            myFontFamily.SelectedItem = temp;
            temp = myEditor.Selection.GetPropertyValue( Inline.FontSizeProperty );
            myFontSize.Text = temp.ToString();
        }

        private void OnFontFamilyChanged( object sender, SelectionChangedEventArgs e )
        {
            if( myFontFamily.SelectedItem != null )
                myEditor.Selection.ApplyPropertyValue( Inline.FontFamilyProperty, myFontFamily.SelectedItem );
        }

        private void OnFontSizeChanged( object sender, TextChangedEventArgs e )
        {
            myEditor.Selection.ApplyPropertyValue( Inline.FontSizeProperty, myFontSize.Text );
        }
    }
}
