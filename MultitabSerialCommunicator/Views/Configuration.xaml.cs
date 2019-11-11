using MultitabSerialCommunicator.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MultitabSerialCommunicator.Views
{
    /// <summary>
    /// Interaction logic for Configuration.xaml
    /// </summary>
    public partial class Configuration : Window
    {
        public Configuration() {
            InitializeComponent();
        }

        public Action<ConfigTypes, int> Callback { get; set; }

        private void Tfwyoucantbebotheredtobindandjustusebehindcode_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
            Callback?.Invoke(ConfigTypes.MaxMessagesReceivable, Convert.ToInt32(tfwyoucantbebotheredtobindandjustusebehindcode.Value));
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            e.Cancel = true;
            this.Hide();
        }
    }
}
