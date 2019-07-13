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
            addNew();
        }

        public int SelectedIndex { get { return main.SelectedIndex; } }

        private void addNew(object sender, RoutedEventArgs e)
        {
            addNew();
        }

        void addNew()
        {
            SerialView sview = new SerialView();
            sview.DataContext = new SerialViewModel();

            TabItem ti = new TabItem();
            ContextMenu cm = new ContextMenu();
            MenuItem mi = new MenuItem();
            mi.Header = "Close";
            mi.Click += Mi_Click;
            mi.Uid = "0";
            cm.Items.Add(mi);

            ti.ContextMenu = cm;
            ti.Header = $"Serial {main.Items.Count}";
            ti.Content = sview;

            main.Items.Add(ti);
            main.SelectedIndex = main.Items.Count - 1;
        }

        private void Mi_Click(object sender, RoutedEventArgs e)
        {
            switch(int.Parse(((MenuItem)e.Source).Uid))
            {
                case 0: main.Items.RemoveAt(main.SelectedIndex); break;
            }
        }
    }
}
