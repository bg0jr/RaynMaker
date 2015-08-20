using System;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using ICSharpCode.AvalonEdit.Folding;
using RaynMaker.Blade.ViewModels;
using System.Linq;
using System.Collections.Generic;
using RaynMaker.Blade.Engine;

namespace RaynMaker.Blade.Views
{
    [Export]
    public partial class AnalysisTemplateEditView : UserControl
    {
        private FoldingManager myFoldingManager;
        private XmlFoldingStrategy myFoldingStrategy;
        private CompletionWindow myCompletionWindow;
        private IEnumerable<CompletionData> myCompletionData;

        [ImportingConstructor]
        internal AnalysisTemplateEditView( AnalysisTemplateEditViewModel viewModel )
        {
            InitializeComponent();

            DataContext = viewModel;

            Loaded += OnLoaded;
        }

        private void OnLoaded( object sender, RoutedEventArgs e )
        {
            myTextEditor.Document.TextChanged += OnTextChanged;
            OnTextChanged( null, null );

            myTextEditor.TextArea.TextEntering += OnTextEntering;
            myTextEditor.TextArea.TextEntered += OnTextEntered;

            myCompletionData = GetType().Assembly.GetTypes()
                .Where( t => t.GetInterfaces().Any( iface => iface == typeof( IReportElement ) ) )
                .Select( t => new CompletionData( t.Name, t.Name ) )
                .ToList();
        }

        private void OnTextChanged( object sender, EventArgs e )
        {
            if( myFoldingManager == null )
            {
                myFoldingManager = FoldingManager.Install( myTextEditor.TextArea );
                myFoldingStrategy = new XmlFoldingStrategy();
            }

            myFoldingStrategy.UpdateFoldings( myFoldingManager, myTextEditor.Document );
        }

        private void OnTextEntering( object sender, TextCompositionEventArgs e )
        {
            if( e.Text.Length > 0 && myCompletionWindow != null )
            {
                if( !char.IsLetterOrDigit( e.Text[ 0 ] ) )
                {
                    // Whenever a non-letter is typed while the completion window is open,
                    // insert the currently selected element.
                    myCompletionWindow.CompletionList.RequestInsertion( e );
                }
            }

            // Do not set e.Handled=true.
            // We still want to insert the character that was typed.
        }

        private void OnTextEntered( object sender, TextCompositionEventArgs e )
        {
            if( e.Text == "<" )
            {
                myCompletionWindow = new CompletionWindow( myTextEditor.TextArea );

                var data = myCompletionWindow.CompletionList.CompletionData;
                foreach( var item in myCompletionData )
                {
                    data.Add( item );
                }
                myCompletionWindow.Show();
                myCompletionWindow.Closed += delegate
                {
                    myCompletionWindow = null;
                };
            }
            else if( e.Text == ">" && myTextEditor.Document.GetCharAt( myTextEditor.CaretOffset - 2 ) != '/' )
            {
                var currentLine = myTextEditor.Document.GetLineByOffset( myTextEditor.CaretOffset );
                var pos = myTextEditor.Document.LastIndexOf( '<', currentLine.Offset, myTextEditor.CaretOffset - currentLine.Offset );
                var xmlTag = myTextEditor.Document.Text.Substring( pos + 1, myTextEditor.CaretOffset - pos - 2 );

                var oldCaretOffset = myTextEditor.CaretOffset;

                myTextEditor.Document.Insert( myTextEditor.CaretOffset, "</" + xmlTag + ">" );

                myTextEditor.CaretOffset = oldCaretOffset;
            }
        }

        public class CompletionData : ICompletionData
        {
            public CompletionData( string text, string description )
            {
                Text = text;
                Description = description;
            }

            public ImageSource Image { get { return null; } }

            public string Text { get; private set; }

            // Use this property if you want to show a fancy UIElement in the list.
            public object Content { get { return Text; } }

            public object Description { get; private set; }

            public void Complete( TextArea textArea, ISegment completionSegment, EventArgs insertionRequestEventArgs )
            {
                textArea.Document.Replace( completionSegment, this.Text );
            }

            public double Priority { get { return 0; } }
        }
    }
}
