using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MultitabSerialCommunicator
{
    public class SerialDev
    {
        readonly SerialPort            serialPort   = new SerialPort();
        readonly SerialMessageListener listener     = new SerialMessageListener();
        readonly SerialSender          serialSender = new SerialSender();
        List<Task>                     tsks         = new List<Task>();
        bool                           valuesSetup  = false;
        bool connected => serialPort.IsOpen;
        public void SetPortValues(string baud, string dbit, string sbit, string prty, string hndk, Encoding encd, string portname, string newLine)
        {
            if (!string.IsNullOrEmpty(baud))     serialPort.BaudRate = int.Parse(baud);
            if (!string.IsNullOrEmpty(dbit))     serialPort.BaudRate = int.Parse(dbit);
            if (!string.IsNullOrEmpty(sbit))     serialPort.StopBits = (StopBits)Enum.Parse(typeof(StopBits), sbit);
            if (!string.IsNullOrEmpty(prty))     serialPort.Parity = (Parity)Enum.Parse(typeof(Parity), prty);
            if (!string.IsNullOrEmpty(hndk))     serialPort.Handshake = (Handshake)Enum.Parse(typeof(Handshake), hndk);
            if (!string.IsNullOrEmpty(portname)) serialPort.PortName = portname;
            serialPort.NewLine = newLine;
            valuesSetup = true;
        }

        public SerialDev()
        {
            serialPort.NewLine = "\n";
            tsks.Add(listener.BeginMessageListener(serialPort));
        }

        public void SetPortName(string Portname)
        {
            if (!string.IsNullOrEmpty(Portname)
                && !connected)
            {
                serialPort.PortName = Portname;
            }
        }

        public void ResetSerialHelpers()
        {
            Task.WhenAll(tsks);
            listener.CloseSerialPort();
            tsks.Add(listener.BeginMessageListener(serialPort));
        }

        public string AutoConnectToArduino()
        {
            if (!valuesSetup) return "Err";
            if (!connected)
            {
                serialPort.Open();
                tsks.Add(listener.BeginMessageListener(serialPort));
                return "Disconnect";
            }
            else
            {
                Task.WhenAll(tsks);
                DisconnectFromArduino(); return "Connect";
            }
        }

        private void DisconnectFromArduino()
        {
            if (connected) serialPort.Close();
            tsks.Add(listener.BeginMessageListener(serialPort));
        }

        public void SendSerialMessage(string data)
        {
            serialSender.SendSerialMessage(serialPort, data);
        }
    }
}
