using System.ComponentModel.Composition;
using System.Windows.Controls;
using RaynMaker.Modules.Notes.ViewModels;

namespace RaynMaker.Modules.Notes.Views
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
