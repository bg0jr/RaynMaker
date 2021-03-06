﻿using System;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using ICSharpCode.AvalonEdit.Folding;
using RaynMaker.Modules.Analysis.ViewModels;
using System.Linq;
using System.Collections.Generic;
using RaynMaker.Modules.Analysis.Engine;
using RaynMaker.Modules.Analysis.AnalysisSpec;

namespace RaynMaker.Modules.Analysis.Views
{
    [Export]
    public partial class AnalysisTemplateEditView : UserControl
    {
        private FoldingManager myFoldingManager;
        private XmlFoldingStrategy myFoldingStrategy;
        private CompletionWindow myCompletionWindow;
        private IEnumerable<KeywordCompletionData> myCompletionData;

        [ImportingConstructor]
        internal AnalysisTemplateEditView( AnalysisTemplateEditViewModel viewModel )
        {
            InitializeComponent();

            DataContext = viewModel;

            Loaded += OnLoaded;
        }

        private void OnLoaded( object sender, RoutedEventArgs e )
        {
            myTextEditor.Options.IndentationSize = 4;
            myTextEditor.Options.ConvertTabsToSpaces = true;

            myTextEditor.Document.TextChanged += OnTextChanged;
            OnTextChanged( null, null );

            myTextEditor.TextArea.TextEntering += OnTextEntering;
            myTextEditor.TextArea.TextEntered += OnTextEntered;

            myCompletionData = GetType().Assembly.GetTypes()
                .Where( t => t.GetInterfaces().Any( iface => iface == typeof( IReportElement ) ) )
                .Concat( new[] { typeof( Row ) } )
                .Select( t => new KeywordCompletionData( t ) )
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
                HandleKeywordCompletion();
            }
            else if( e.Text == ">" )
            {
                HandleTagClosing();
            }
            else if( e.Text == " " )
            {
                HandlePropertyCompletion();
            }
        }

        private void HandleKeywordCompletion()
        {
            ShowCompletionWindow( myCompletionData );
        }

        private void ShowCompletionWindow( IEnumerable<ICompletionData> competionItems )
        {
            myCompletionWindow = new CompletionWindow( myTextEditor.TextArea );

            var data = myCompletionWindow.CompletionList.CompletionData;
            foreach( var item in competionItems )
            {
                data.Add( item );
            }
            myCompletionWindow.Show();
            myCompletionWindow.Closed += delegate
            {
                myCompletionWindow = null;
            };
        }

        private void HandleTagClosing()
        {
            if( myTextEditor.Document.GetCharAt( myTextEditor.CaretOffset - 2 ) == '/' )
            {
                return;
            }

            var currentLine = myTextEditor.Document.GetLineByOffset( myTextEditor.CaretOffset );
            var lastOpenedTagPos = myTextEditor.Document.LastIndexOf( '<', currentLine.Offset, myTextEditor.CaretOffset - currentLine.Offset );
            var spaceAfterOpenedTagPos = myTextEditor.Document.IndexOf( ' ', lastOpenedTagPos, myTextEditor.CaretOffset - lastOpenedTagPos );
            var xmlTag = myTextEditor.Document.Text.Substring( lastOpenedTagPos + 1, spaceAfterOpenedTagPos - lastOpenedTagPos - 1 );

            var oldCaretOffset = myTextEditor.CaretOffset;

            myTextEditor.Document.Insert( myTextEditor.CaretOffset, "</" + xmlTag + ">" );

            myTextEditor.CaretOffset = oldCaretOffset;
        }

        private void HandlePropertyCompletion()
        {
            var lastClosedTagPos = myTextEditor.Document.LastIndexOf( '>', 0, myTextEditor.CaretOffset );
            if( lastClosedTagPos < 0 )
            {
                lastClosedTagPos = 0;
            }

            var lastOpenedTagPos = myTextEditor.Document.LastIndexOf( '<', lastClosedTagPos, myTextEditor.CaretOffset - lastClosedTagPos );
            if( lastOpenedTagPos < 0 )
            {
                return;
            }

            var spaceAfterOpenedTagPos = myTextEditor.Document.IndexOf( ' ', lastOpenedTagPos, myTextEditor.CaretOffset - lastOpenedTagPos );
            if( spaceAfterOpenedTagPos < 0 )
            {
                return;
            }

            var xmlTag = myTextEditor.Document.Text.Substring( lastOpenedTagPos + 1, spaceAfterOpenedTagPos - lastOpenedTagPos - 1 );
            var completionData = myCompletionData.SingleOrDefault( d => d.Type.Name == xmlTag );
            if( completionData == null )
            {
                return;
            }

            var completionItems = completionData.Type.GetProperties()
                .Select( p => new PropertyCompletionData( p ) );
            ShowCompletionWindow( completionItems );
        }
    }
}
