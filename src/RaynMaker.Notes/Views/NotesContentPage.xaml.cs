using System.ComponentModel.Composition;
using System.Windows.Controls;
using Microsoft.Practices.Prism.Regions;
using RaynMaker.Notes.ViewModels;

namespace RaynMaker.Notes.Views
{
    [Export]
    [ViewSortHint( "150" )]
    public partial class NotesContentPage : UserControl
    {
        [ImportingConstructor]
        internal NotesContentPage( NotesContentPageModel viewModel )
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
