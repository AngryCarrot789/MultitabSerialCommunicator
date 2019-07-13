using MahApps.Metro.Controls;
using MultitabSerialCommunicator.Views;
using System;
using System.Collections.Generic;
using System.IO.Ports;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MultitabSerialCommunicator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public int SelectedIndex { get { return main.SelectedIndex; } }

        private void addNew(object sender, RoutedEventArgs e)
        {
            SerialView sview = new SerialView();
            sview.DataContext = new SerialViewModel();

            TabItem ti = new TabItem();
            ti.Header = $"Serial {main.Items.Count}";
            ti.Content = sview;

            main.Items.Add(ti);
        }
    }
}
