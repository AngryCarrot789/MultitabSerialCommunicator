using MultitabSerialCommunicator.ViewModels;
using MultitabSerialCommunicator.Views;
using System;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MultitabSerialCommunicator
{
    public class SerialViewModel : BaseViewModel
    {
        #region Private fields
        private int baudRate;
        private string portName;
        private string dataBits;
        private string stopBits;
        private string parity;
        private string handShake;
        private ObservableCollection<string> ports = new ObservableCollection<string>();
        private string mainText;
        private string sendText;
        private string btnText;
        private int readTimeout;
        private int writeTimeout;
        private int bufferSize = 4096;
        private bool dtrEnable;
        private bool autoScroll;
        private SerialDev serialDev = new SerialDev();
        readonly ISerialModel iSerial;
        public SerialDataCollections SerialDataCollections { get; set; } = new SerialDataCollections();
        #endregion

        #region public fields
        public Action<bool> SetAutoscroll { get; set; }
        public int    SVMBaudRate                 { get { return baudRate;  }    set { baudRate = value;     refreshVariables(); RaisePropertyChanged(); } }
        public string SVMPortName                 { get { return portName;  }    set { portName = value;     refreshVariables(); RaisePropertyChanged(); } }
        public string SVMDataBits                 { get { return dataBits;  }    set { dataBits = value;     refreshVariables(); RaisePropertyChanged(); } }
        public string SVMStopbits                 { get { return stopBits;  }    set { stopBits = value;     refreshVariables(); RaisePropertyChanged(); } }
        public string SVMParity                   { get { return parity;    }    set { parity = value;       refreshVariables(); RaisePropertyChanged(); } }
        public string SVMHandShake                { get { return handShake; }    set { handShake = value;    refreshVariables(); RaisePropertyChanged(); } }
        public ObservableCollection<string> Ports { get { return ports;     }    set { ports = value;        RaisePropertyChanged(); } }
        public string MainText                    { get { return mainText; }     set { mainText = value;     RaisePropertyChanged(); } }
        public string SendText                    { get { return sendText; }     set { sendText = value;     RaisePropertyChanged(); } }
        public string ButtonText                  { get { return btnText; }      set { btnText = value;      RaisePropertyChanged(); } }
        public int ReadTimeout                    { get { return readTimeout; }  set { readTimeout = value;  RaisePropertyChanged(); } }
        public int WriteTimeout                   { get { return writeTimeout; } set { writeTimeout = value; RaisePropertyChanged(); } }
        public int BufferSize                     { get { return bufferSize; }   set { bufferSize = value;   RaisePropertyChanged(); } }
        public bool DTREnable                     { get { return dtrEnable; }    set { dtrEnable = value;    serialDev.UpdateDTR(value);    RaisePropertyChanged(); } }
        public bool AutoScroll                    { get { return autoScroll; }   set { autoScroll = value;   SetAutoscroll?.Invoke(value);  RaisePropertyChanged(); } }
        public ICommand ConnectToPort     { get; set; }
        public ICommand SendSerialMessage { get; set; }
        public ICommand RefreshCOMsList   { get; set; }
        public ICommand ClearBuffers      { get; set; }
        public ICommand ClearText         { get; set; }
        #endregion

        #region Constructor

        public SerialViewModel(SerialView uC)
        {
            iSerial = this.serialDev;
            iSerial.OnMessage = message;
            AutoScroll = true;
            ConnectToPort         = new DelegateCommand(connect);
            SendSerialMessage     = new DelegateCommand(sendMessage);
            RefreshCOMsList       = new DelegateCommand(refreshList);
            ClearBuffers          = new DelegateCommand(clrBuffers);
            ClearText             = new DelegateCommand(clrMessageBuffer);
            SVMBaudRate           = 115200;
            SVMDataBits           = "8";
            SVMStopbits           = "One";
            SVMParity             = "None";
            SVMHandShake          = "None";
            ButtonText            = "Connect";
            ReadTimeout           = 500;
            WriteTimeout          = 500;
            BufferSize            = 4096;

            this.serialDev.SetTimeouts(ReadTimeout, WriteTimeout);
            refreshList();
            refreshVariables();
        }

        #endregion

        #region Methods

        private void refreshVariables()
        {
            this.serialDev.SetPortValues(SVMBaudRate.ToString(),
                                         SVMDataBits,
                                         SVMStopbits,
                                         SVMParity,
                                         SVMHandShake,
                                         Encoding.ASCII,
                                         SVMPortName,
                                         BufferSize);
        }

        private void message(string data, string rxortx)
        {
            AddNewMessage(data, rxortx);
        }

        private void clrMessageBuffer()
        {
            MainText = "";
        }
        private void clrBuffers()
        {
            serialDev.ClearBuffers();
        }

        private void connect()
        {
            string status = serialDev.AutoConnectToArduino();
            ButtonText = status;
            if (status == "Disconnect")
            {
                startupMessage(serialDev.EPortname, serialDev.EBaudrate, serialDev.EDatabits);
            }
        }

        private void sendMessage()
        {
            serialDev.SendSerialMessage(SendText);
        }

        private void refreshList()
        {
            Ports.Clear();
            foreach (string v in SerialPort.GetPortNames()) Ports.Add(v);
        }

        public void AddNewMessage(string data, string RXorTX)
        {
            MainText += $"{RXorTX}> {data}" + '\n';
        }

        private void startupMessage(string portname, string baudrate, string databits)
        {
            MainText += $"Port: {portname}{Environment.NewLine}Baudrate: {baudrate}{Environment.NewLine}DataBits: {databits}{Environment.NewLine}";
        }

        public void DisposeProcedure()
        {
            serialDev.DisposeProc();
            serialDev = null;
        }

        #endregion
    }
}
