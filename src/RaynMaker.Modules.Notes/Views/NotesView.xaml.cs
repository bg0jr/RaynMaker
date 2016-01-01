using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using RaynMaker.Modules.Notes.ViewModels;

namespace RaynMaker.Modules.Notes.Views
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
