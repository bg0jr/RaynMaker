using System.ComponentModel.Composition;
using RaynMaker.Infrastructure;
using RaynMaker.Infrastructure.Mvvm;

namespace RaynMaker.Notes.ViewModels
{
    [Export]
    public class NotesMenuItemModel : ToolMenuItemModelBase
    {
        [ImportingConstructor]
        public NotesMenuItemModel( IProjectHost projectHost )
            : base( projectHost, "Notes" )
        {
        }
    }
}
