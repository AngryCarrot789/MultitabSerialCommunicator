using System.Windows.Controls;

namespace MultitabSerialCommunicator.Views
{
    /// <summary>
    /// Interaction logic for SerialView.xaml
    /// </summary>
    public partial class SerialView : UserControl
    {
        public SerialView()
        {
            InitializeComponent();
            SerialViewModel svm = new SerialViewModel(new SerialDev());
            this.DataContext = svm;
        }
    }
}
