using System;
using System.Windows;
using System.Windows.Controls;

namespace MultitabSerialCommunicator.Views
{
    /// <summary>
    /// Interaction logic for SerialView.xaml
    /// </summary>
    public partial class SerialView : UserControl
    {
        public SerialView() {
            InitializeComponent();
            SerialViewModel svm = new SerialViewModel();
            this.DataContext = svm;

            svm.SetAutoscroll = setAutoScroll;
        }

        private void setAutoScroll(bool val) => autoscrollEnabled = val;

        bool autoscrollEnabled = true;

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e) {
            if (autoscrollEnabled)
                ((TextBox)e.Source).ScrollToEnd();

        }
    }
}