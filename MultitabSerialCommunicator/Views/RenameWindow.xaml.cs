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
    /// Interaction logic for RenameWindow.xaml
    /// </summary>
    public partial class RenameWindow : Window
    {
        public TabItem Tab { get; set; }
        public RenameWindow()
        {
            InitializeComponent();
        }

        public void Display()
        {
            this.Show();
            rnameTxt.Text = "";
            rnameTxt.Focus();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        public void RenameTab()
        {
            try
            {
                if (Tab != null)
                    Tab.Header = rnameTxt.Text;
            }
            catch { }
            this.Hide();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RenameTab();
        }

        private void rnameTxt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                RenameTab();
            if (e.Key == Key.Escape)
                this.Hide();
        }
    }
}
