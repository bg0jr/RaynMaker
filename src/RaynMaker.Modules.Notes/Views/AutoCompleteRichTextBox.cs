using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Navigation;

namespace RaynMaker.Modules.Notes.Views
{
    public class AutoCompleteRichTextBox : RichTextBox
    {
        // Private flag - true when word(s) are added to this RichTextBox.
        private bool myWordsAddedFlag;

        // TextPointers that track the range covering content where words are added.
        private TextPointer mySelectionStartPosition;
        private TextPointer mySelectionEndPosition;

        static AutoCompleteRichTextBox()
        {
            EventManager.RegisterClassHandler( typeof( AutoCompleteRichTextBox ), KeyDownEvent, new KeyEventHandler( OnKeyDown ), handledEventsToo: true );
        }

        public AutoCompleteRichTextBox()
        {
            DataObject.AddPastingHandler( this, OnPasted );

            TextChanged += OnTextChanged;

            // required to get hyperlinks working
            IsDocumentEnabled = true;
        }

        private static void OnKeyDown( object sender, KeyEventArgs e )
        {
            var richTextBox = ( AutoCompleteRichTextBox )sender;

            if( e.Key != Key.Back && e.Key != Key.Space && e.Key != Key.Return )
            {
                return;
            }

            if( !richTextBox.Selection.IsEmpty )
            {
                richTextBox.Selection.Text = String.Empty;
            }

            var caretPosition = richTextBox.Selection.Start;

            if( e.Key == Key.Space || e.Key == Key.Return )
            {
                // We don't check for hyperlink match here, just set the necessary flags.
                // Once base RTB handles the KeyDown event, it will raise a TextChanged event, 
                // in which we check for a hyperlink match in affected range.

                richTextBox.myWordsAddedFlag = true;
                richTextBox.mySelectionStartPosition = richTextBox.Selection.Start;
                richTextBox.mySelectionEndPosition = richTextBox.Selection.End.GetPositionAtOffset( 0, LogicalDirection.Forward );
            }
            else // Key.Back
            {
                RemoveHyperlink( richTextBox, caretPosition );
            }
        }

        private static void RemoveHyperlink( AutoCompleteRichTextBox myRichTextBox, TextPointer caretPosition )
        {
            var backspacePosition = caretPosition.GetNextInsertionPosition( LogicalDirection.Backward );
            Hyperlink hyperlink;
            if( backspacePosition == null || !DocumentUtils.IsHyperlinkBoundaryCrossed( caretPosition, backspacePosition, out hyperlink ) )
            {
                return;
            }

            // Remember caretPosition with forward gravity. This is necessary since we are going to delete 
            // the hyperlink element preceeding caretPosition and after deletion current caretPosition 
            // (with backward gravity) will follow content preceeding the hyperlink. 
            // We want to remember content following the hyperlink to set new caret position at.

            var newCaretPosition = caretPosition.GetPositionAtOffset( 0, LogicalDirection.Forward );

            // Deleting the hyperlink is done using logic below.

            // 1. Copy its children Inline to a temporary array.
            var hyperlinkChildren = hyperlink.Inlines;
            var inlines = new Inline[ hyperlinkChildren.Count ];
            hyperlinkChildren.CopyTo( inlines, 0 );

            // 2. Remove each child from parent hyperlink element and insert it after the hyperlink.
            for( int i = inlines.Length - 1; i >= 0; i-- )
            {
                hyperlinkChildren.Remove( inlines[ i ] );
                hyperlink.SiblingInlines.InsertAfter( hyperlink, inlines[ i ] );
            }

            // 3. Apply hyperlink's local formatting properties to inlines (which are now outside hyperlink scope).
            var localProperties = hyperlink.GetLocalValueEnumerator();
            var inlineRange = new TextRange( inlines[ 0 ].ContentStart, inlines[ inlines.Length - 1 ].ContentEnd );

            while( localProperties.MoveNext() )
            {
                var property = localProperties.Current;
                var dp = property.Property;
                object value = property.Value;

                if( !dp.ReadOnly &&
                    dp != Inline.TextDecorationsProperty && // Ignore hyperlink defaults.
                    dp != TextElement.ForegroundProperty &&
                    dp != BaseUriHelper.BaseUriProperty &&
                    !DocumentUtils.IsHyperlinkProperty( dp ) )
                {
                    inlineRange.ApplyPropertyValue( dp, value );
                }
            }

            // 4. Delete the (empty) hyperlink element.
            hyperlink.SiblingInlines.Remove( hyperlink );

            // 5. Update selection, since we deleted Hyperlink element and caretPosition was at that Hyperlink's end boundary.
            myRichTextBox.Selection.Select( newCaretPosition, newCaretPosition );
        }

        private void OnPasted( object sender, DataObjectPastingEventArgs e )
        {
            myWordsAddedFlag = true;
            mySelectionStartPosition = Selection.Start;
            mySelectionEndPosition = Selection.IsEmpty ?
                Selection.End.GetPositionAtOffset( 0, LogicalDirection.Forward ) :
                Selection.End;

            // We don't handle the event here. Let the base RTB handle the paste operation.
            // This will raise a TextChanged event, which we handle below to scan for any matching hyperlinks.
        }

        private void OnTextChanged( object sender, TextChangedEventArgs e )
        {
            if( !myWordsAddedFlag || Document == null )
            {
                return;
            }

            // Temporarily disable TextChanged event handler, since following code might insert Hyperlinks,
            // which will raise another TextChanged event.
            TextChanged -= OnTextChanged;

            var navigator = mySelectionStartPosition;
            while( navigator != null && navigator.CompareTo( mySelectionEndPosition ) <= 0 )
            {
                var wordRange = DocumentUtils.GetWordRange( navigator );
                if( wordRange == null || wordRange.IsEmpty )
                {
                    // No more words in the document.
                    break;
                }

                string wordText = wordRange.Text;
                if( IsHyperlink( wordText ) &&
                    !DocumentUtils.IsInHyperlinkScope( wordRange.Start ) &&
                    !DocumentUtils.IsInHyperlinkScope( wordRange.End ) )
                {
                    var hyperlink = new Hyperlink( wordRange.Start, wordRange.End );
                    hyperlink.NavigateUri = new Uri( wordText.StartsWith( "http", StringComparison.OrdinalIgnoreCase ) ? wordText : "http://" + wordText );
                    WeakEventManager<Hyperlink, RequestNavigateEventArgs>.AddHandler( hyperlink, "RequestNavigate", OnHyperlinkRequestNavigate );

                    navigator = hyperlink.ElementEnd.GetNextInsertionPosition( LogicalDirection.Forward );
                }
                else
                {
                    navigator = wordRange.End.GetNextInsertionPosition( LogicalDirection.Forward );
                }
            }

            TextChanged += OnTextChanged;

            myWordsAddedFlag = false;
            mySelectionStartPosition = null;
            mySelectionEndPosition = null;
        }

        private void OnHyperlinkRequestNavigate( object sender, RequestNavigateEventArgs e )
        {
            Process.Start( e.Uri.AbsoluteUri );
            e.Handled = true;
        }

        private bool IsHyperlink( string wordText )
        {
            return wordText.StartsWith( "http://", StringComparison.OrdinalIgnoreCase ) ||
                wordText.StartsWith( "https://", StringComparison.OrdinalIgnoreCase ) ||
                wordText.StartsWith( "www.", StringComparison.OrdinalIgnoreCase );
        }
    }
}
