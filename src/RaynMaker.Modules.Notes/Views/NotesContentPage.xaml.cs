using System.ComponentModel.Composition;
using System.Windows.Controls;
using Microsoft.Practices.Prism.Regions;
using RaynMaker.Modules.Notes.ViewModels;

namespace RaynMaker.Modules.Notes.Views
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
