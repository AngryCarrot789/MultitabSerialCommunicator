using MultitabSerialCommunicator.Views;
using System;
using System.Windows;
using System.Windows.Controls;

namespace MultitabSerialCommunicator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public static bool[] KeysDown = new bool[200];

        private void Window_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            KeysDown[Convert.ToInt32(e.Key)] = true;
            (DataContext as MainViewModel).KeyDown(e.Key, KeysDown);
        }

        private void Window_PreviewKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            KeysDown[Convert.ToInt32(e.Key)] = false;
            Array.Clear(KeysDown, 0, KeysDown.Length);
        }
    }
}