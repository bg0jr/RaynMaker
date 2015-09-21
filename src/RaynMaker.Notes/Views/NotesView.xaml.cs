using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using RaynMaker.Notes.ViewModels;

namespace RaynMaker.Notes.Views
{
    [Export]
    public partial class NotesView : UserControl
    {
        [ImportingConstructor]
        internal NotesView( NotesViewModel viewModel )
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
