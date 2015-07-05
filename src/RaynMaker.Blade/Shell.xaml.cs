using System.ComponentModel.Composition;
using System.Windows;

namespace RaynMaker.Blade
{
    [Export]
    public partial class Shell : Window
    {
        [ImportingConstructor]
        internal Shell( ShellViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
