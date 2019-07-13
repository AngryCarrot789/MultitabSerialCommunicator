using MultitabSerialCommunicator.Views;
using System;
using System.IO.Ports;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MultitabSerialCommunicator
{
    public class SerialSender
    {
        private SerialPort sport = new SerialPort();
        public void SendSerialMessage(SerialPort sPort, string data)
        {
            sport = sPort;
            if (sport.IsOpen)
            {
                Task.Run(() =>
                {
                    try { sport.WriteLine(data); }
                    catch(TimeoutException) { }
                    catch(Exception gg) { MessageBox.Show(gg.Message); }
                });
                addNewMessage(data);
            }
        }

        public SerialSender()
        {

        }

        public void CloseSerialPort()
        {
            try { sport.Close(); } catch(Exception g) { MessageBox.Show(g.Message); }
        }

        private void addNewMessage(string data)
        {
            //SerialViewModel.serialViewModel.AddNewMessage("TX", data);

            var hwnd = (MainWindow)Application.Current.MainWindow;
            (((hwnd.main.Items.GetItemAt(hwnd.SelectedIndex) as TabItem).Content as SerialView).DataContext as SerialViewModel).AddNewMessage("TX", data);
        }
    }
}
