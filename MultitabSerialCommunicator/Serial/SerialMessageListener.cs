using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MultitabSerialCommunicator
{
    public class SerialMessageListener
    {
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
                        addNewMessage(sport.ReadLine());
                    }
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
            SerialViewModel.serialViewModel.AddNewMessage("RX", data);
        }
    }
}
