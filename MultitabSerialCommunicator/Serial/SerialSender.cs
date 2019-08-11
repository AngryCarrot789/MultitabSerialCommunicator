using MultitabSerialCommunicator.Views;
using System;
using System.IO.Ports;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MultitabSerialCommunicator
{
    public class SerialSender : ISerialModel, ISerialComms
    {
        private SerialPort sport = new SerialPort();

        public Action<string, string> OnMessage { get; set; }

        public void SendSerialMessage(SerialPort sPort, string data)
        {
            sport = sPort;
            if (sport.IsOpen)
            {
                Task.Run(() =>
                {
                    sport.NewLine = "\n";
                    try { sport.WriteLine(data); }
                    catch(TimeoutException) { }
                    catch(Exception gg) { MessageBox.Show(gg.Message); }
                });
                addNewMessage(data);
            }
        }

        public void CloseSerialPort()
        {
            try { sport.Close(); } catch(Exception g) { MessageBox.Show(g.Message); }
        }

        private void addNewMessage(string data)
        {
            NewMessage(data, "TX");
        }

        public void DisposeProc()
        {
            sport.Dispose();
        }

        public void NewMessage(string data, string RX_or_TX)
        {
            OnMessage?.Invoke(data, RX_or_TX);
        }
    }
}
