using System.Windows;
using System.Windows.Documents;

namespace RaynMaker.Blade.Reporting
{
    public partial class ReportView : Window
    {
        public ReportView()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty DocumentProperty = DependencyProperty.Register( "Document", typeof( FlowDocument ),
            typeof( ReportView ), new FrameworkPropertyMetadata( null ) );

        public FlowDocument Document
        {
            get { return ( FlowDocument )GetValue( DocumentProperty ); }
            set { SetValue( DocumentProperty, value ); }
        }
    }
}
