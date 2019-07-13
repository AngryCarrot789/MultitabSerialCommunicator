using MultitabSerialCommunicator.Views;
using System;
using System.IO;
using System.IO.Ports;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MultitabSerialCommunicator
{
    public class SerialMessageListener
    {
        public SerialMessageListener() { }
        private SerialPort sport = new SerialPort();
        public async Task BeginMessageListener(SerialPort sPort)
        {
            await Task.Run(() =>
            {
                sport = sPort;
                while (sport.IsOpen)
                {
                    try
                    {
                        if (sport.BytesToRead > 0)
                            addNewMessage(sport.ReadLine());
                    }
                    catch (TimeoutException) { }
                    catch (InvalidOperationException g) { MessageBox.Show(g.Message); }
                    catch (IOException) { }
                }
            });
        }

        public void CloseSerialPort()
        {
            try { sport.Close(); } catch(Exception gg) { MessageBox.Show(gg.Message); }
        }
        //uses dependency injection
        private void addNewMessage(string data)
        {
            //SerialViewModel.serialViewModel.AddNewMessage("RX", data);
            Application.Current.Dispatcher.InvokeAsync(() =>
            {
                var hwnd = (MainWindow)Application.Current.MainWindow;
                (((hwnd.main.Items.GetItemAt(hwnd.SelectedIndex) as TabItem).Content as SerialView).DataContext as SerialViewModel).AddNewMessage("RX", data);
            });
        }

        public void DisposeProc()
        {
            sport.Dispose();
        }
    }
}
