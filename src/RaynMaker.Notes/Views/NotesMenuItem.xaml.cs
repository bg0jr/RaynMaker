using System.ComponentModel.Composition;
using System.Windows.Controls;
using RaynMaker.Notes.ViewModels;

namespace RaynMaker.Notes.Views
{
    [Export]
    public partial class NotesMenuItem : MenuItem
    {
        [ImportingConstructor]
        public NotesMenuItem( NotesMenuItemModel model )
        {
            InitializeComponent();

            DataContext = model;
        }
    }
}
