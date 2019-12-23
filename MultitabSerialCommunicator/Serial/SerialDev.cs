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
        readonly SerialPort serialPort = new SerialPort();
        readonly SerialSender serialSender = new SerialSender();
        bool valuesSetup = false;

        bool _connected => serialPort.IsOpen;

        public string EPortname => serialPort.PortName;
        public string EBaudrate => serialPort.BaudRate.ToString();
        public string EDatabits => serialPort.DataBits.ToString();

        public SerialDev()
        {
            serialPort.NewLine = "\n";
            serialPort.DtrEnable = true;
            serialPort.DataReceived += SerialPort_DataReceived;
            serialSender.OnMessage = newMessge;
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                newMessge(serialPort.ReadLine(), "RX");
            }
            catch(TimeoutException) { }
            catch(Exception ed) { newMessge(ed.Message, "Exception"); } 
        }

        public Action<string, string> OnMessage { get; set; }
        public Action<string> MessageCallback { get; set; }

        private void newMessge(string data, string txrx)
        {
            OnMessage?.Invoke(data, txrx);
        }

        public void SetPortValues(string baud, string dbit, string sbit, string prty, string hndk, string portname, int bufferSize)
        {
            //return if conencted.
            if (_connected) {
                CallbackMSG("Connected to a port. Cannon change values.");
                return;
            }

            if (!string.IsNullOrEmpty(baud))
                serialPort.BaudRate = int.Parse(baud);
            if (!string.IsNullOrEmpty(dbit))
                serialPort.DataBits = int.Parse(dbit);
            if (!string.IsNullOrEmpty(sbit))
                serialPort.StopBits = (StopBits)Enum.Parse(typeof(StopBits), sbit);
            if (!string.IsNullOrEmpty(prty))
                serialPort.Parity = (Parity)Enum.Parse(typeof(Parity), prty);
            if (!string.IsNullOrEmpty(hndk))
                serialPort.Handshake = (Handshake)Enum.Parse(typeof(Handshake), hndk);
            if (!string.IsNullOrEmpty(portname))
                serialPort.PortName = portname;
            serialPort.ReadBufferSize = bufferSize;
            serialPort.WriteBufferSize = bufferSize;
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

        //public void ResetSerialHelpers()
        //{
        //    Task.WhenAll(tsks);
        //    listener.CloseSerialPort();
        //    tsks.Add(listener.BeginMessageListener(serialPort));
        //}

        private void CallbackMSG(string msg) => MessageCallback?.Invoke(msg);

        public string AutoConnect()
        {
            if (!valuesSetup) {
                CallbackMSG("Values not Setup. Internal Error");
                return "Err";
            }
            if (!_connected) {
                CallbackMSG("Connecting...");
                try {
                    serialPort.Open();
                    CallbackMSG("Connected.");
                }
                catch (Exception e) { CallbackMSG(e.Message); return "Failed."; }
                if (serialPort.BytesToRead > 0) {
                    newMessge(serialPort.ReadExisting(), "Buffer");
                }
                serialPort.DataReceived += SerialPort_DataReceived;
                return "Disconnect";
            }
            else {
                Disconnect();
                return "Connect";
            }
        }

        private void Disconnect()
        {
            if (_connected)
                try { serialPort.Close(); CallbackMSG("Disconnected"); }
                catch (Exception e) { CallbackMSG(e.Message); return; }
            serialSender.CloseSerialPort();
            //tsks.Add(listener.BeginMessageListener(serialPort));
        }

        public void SendSerialMessage(string data)
        {
            serialSender.SendSerialMessage(serialPort, data);
        }

        public void ClearBuffers()
        {
            if (_connected) {
                serialPort.DiscardInBuffer();
                serialPort.DiscardOutBuffer();
            }
        }

        public void DisposeProc()
        {
            serialPort.Dispose();
            serialSender.DisposeProc();
        }
    }
}