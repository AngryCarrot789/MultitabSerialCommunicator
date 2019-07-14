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
        public void SetPortValues(string baud, string dbit, string sbit, string prty, string hndk, Encoding encd, string portname, int bufferSize)
        {
            if (!string.IsNullOrEmpty(baud))     serialPort.BaudRate = int.Parse(baud);
            if (!string.IsNullOrEmpty(dbit))     serialPort.DataBits = int.Parse(dbit);
            if (!string.IsNullOrEmpty(sbit))     serialPort.StopBits = (StopBits)Enum.Parse(typeof(StopBits), sbit);
            if (!string.IsNullOrEmpty(prty))     serialPort.Parity = (Parity)Enum.Parse(typeof(Parity), prty);
            if (!string.IsNullOrEmpty(hndk))     serialPort.Handshake = (Handshake)Enum.Parse(typeof(Handshake), hndk);
            if (!string.IsNullOrEmpty(portname)) serialPort.PortName = portname;
            serialPort.ReadBufferSize = bufferSize; serialPort.WriteBufferSize = bufferSize;
            serialPort.NewLine = "\n";
            valuesSetup = true;
        }

        public void SetTimeouts(int receive, int transmit)
        {
            serialPort.ReadTimeout = receive;
            serialPort.WriteTimeout = transmit;
        }

        public void UpdateDTR(bool dtrStatus)
        { 
            serialPort.DtrEnable = dtrStatus;
        }

        public SerialDev()
        {
            serialPort.NewLine = "\n";
            tsks.Add(listener.BeginMessageListener(serialPort));
            serialPort.DtrEnable = true;
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
                try
                {
                    serialPort.Open();
                }
                catch { return "Failed."; }
                if (serialPort.BytesToRead > 0)
                {
                    listener.AddSerialMessageBypass(serialPort.ReadExisting());
                }
                tsks.Add(listener.BeginMessageListener(serialPort));
                return "Disconnect";
            }
            else
            {
                DisconnectFromArduino(); return "Connect";
            }
        }

        private void DisconnectFromArduino()
        {
            Task.WhenAll(tsks);
            if (connected) try { serialPort.Close(); } catch { return; }
            listener.CloseSerialPort();
            serialSender.CloseSerialPort();
            //tsks.Add(listener.BeginMessageListener(serialPort));
        }

        public void SendSerialMessage(string data)
        {
            serialSender.SendSerialMessage(serialPort, data);
        }

        public void ClearBuffers()
        {
            if (connected)
            {
                serialPort.DiscardInBuffer();
                serialPort.DiscardOutBuffer();
            }
        }

        public void DisposeProc()
        {
            Task.Run(() =>
            {
                serialPort.Dispose();
                listener.DisposeProc();
                serialSender.DisposeProc();
            });
            Task.WhenAll(tsks);
        }
    }
}
