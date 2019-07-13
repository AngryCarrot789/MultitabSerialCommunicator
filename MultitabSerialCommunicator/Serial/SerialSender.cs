using System;
using System.IO.Ports;
using System.Windows;

namespace MultitabSerialCommunicator
{
    public class SerialSender
    {
        private SerialPort sport = new SerialPort();
        public void SendSerialMessage(SerialPort sPort, string data)
        {
            sport = sPort;
            sport.WriteLine(data);
            addNewMessage(data);
        }

        public void CloseSerialPort()
        {
            try { sport.Close(); } catch(Exception g) { MessageBox.Show(g.Message); }
        }

        private void addNewMessage(string data)
        {
            SerialViewModel.serialViewModel.AddNewMessage("TX", data);
        }
    }
}
