using MultitabSerialCommunicator.Views;
using System;
using System.IO;
using System.IO.Ports;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MultitabSerialCommunicator
{
    public class SerialMessageListener : ISerialModel
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
                        {
                            sport.NewLine = "\n";
                            addNewMessage(sport.ReadLine(), "RX");
                        }
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
        private void addNewMessage(string data, string type)
        {
            OnMessage?.Invoke(data, type);
        }

        public void AddSerialMessageBypass(string data)
        {
            addNewMessage(data, "Buffer");
        }

        public void DisposeProc()
        {
            sport.Dispose();
        }
        public Action<string, string> OnMessage { get; set; }
    }
}
