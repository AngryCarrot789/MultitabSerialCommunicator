using MultitabSerialCommunicator.ViewModels;
using MultitabSerialCommunicator.Views;
using Ookii.Dialogs.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Ports;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;

namespace MultitabSerialCommunicator
{
    public class SerialViewModel : BaseViewModel
    {
        #region Private fields
        private bool fileStreamOpen;
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
        //readonly ISerialModel iSerial;
        public SerialDataCollections SerialDataCollections { get; set; } = new SerialDataCollections();
        #endregion

        #region public fields
        public bool WritingStreamOpen { get => fileStreamOpen; set { fileStreamOpen = value; RaisePropertyChanged(); } }
        public Action<bool> SetAutoscroll { get; set; }
        public int SVMBaudRate { get { return baudRate; } set { baudRate = value; refreshVariables(); RaisePropertyChanged(); } }
        public string SVMPortName { get { return portName; } set { portName = value; refreshVariables(); RaisePropertyChanged(); } }
        public string SVMDataBits { get { return dataBits; } set { dataBits = value; refreshVariables(); RaisePropertyChanged(); } }
        public string SVMStopbits { get { return stopBits; } set { stopBits = value; refreshVariables(); RaisePropertyChanged(); } }
        public string SVMParity { get { return parity; } set { parity = value; refreshVariables(); RaisePropertyChanged(); } }
        public string SVMHandShake { get { return handShake; } set { handShake = value; refreshVariables(); RaisePropertyChanged(); } }
        public ObservableCollection<string> Ports { get { return ports; } set { ports = value; RaisePropertyChanged(); } }
        public string MainText { get { return mainText; } set { mainText = value; RaisePropertyChanged(); } }
        public string SendText { get { return sendText; } set { sendText = value; RaisePropertyChanged(); } }
        public string ButtonText { get { return btnText; } set { btnText = value; RaisePropertyChanged(); } }
        public int ReadTimeout { get { return readTimeout; } set { readTimeout = value; RaisePropertyChanged(); } }
        public int WriteTimeout { get { return writeTimeout; } set { writeTimeout = value; RaisePropertyChanged(); } }
        public int BufferSize { get { return bufferSize; } set { bufferSize = value; RaisePropertyChanged(); } }
        public bool DTREnable { get { return dtrEnable; } set { dtrEnable = value; serialDev.UpdateDTR(value); RaisePropertyChanged(); } }
        public bool AutoScroll { get { return autoScroll; } set { autoScroll = value; SetAutoscroll?.Invoke(value); RaisePropertyChanged(); } }

        public int MessageCount { get; set; }
        public int CurrentMessageCount { get; set; }

        public ICommand ConnectToPort { get; set; }
        public ICommand SendSerialMessage { get; set; }
        public ICommand RefreshCOMsList { get; set; }
        public ICommand ClearBuffers { get; set; }
        public ICommand ClearText { get; set; }
        #endregion

        public bool StreamHasPath;
        public string StreamPath;
        public FILESTREAM WriteStream;

        #region Constructor

        public SerialViewModel()
        {
            MessageCount = 200;

            serialDev.OnMessage = message;
            serialDev.MessageCallback = AddMessage;

            AutoScroll = true;
            ConnectToPort = new DelegateCommand(connect);
            SendSerialMessage = new DelegateCommand(sendMessage);
            RefreshCOMsList = new DelegateCommand(refreshList);
            ClearBuffers = new DelegateCommand(clrBuffers);
            ClearText = new DelegateCommand(clrMessageBuffer);
            SVMBaudRate = 115200;
            SVMDataBits = "8";
            SVMStopbits = "One";
            SVMParity = "None";
            SVMHandShake = "None";
            ButtonText = "Connect";
            ReadTimeout = 500;
            WriteTimeout = 500;
            BufferSize = 4096;

            this.serialDev.SetTimeouts(ReadTimeout, WriteTimeout);
            refreshList();
            refreshVariables();
        }

        #endregion

        #region Methods

        private void refreshVariables()
        {
            this.serialDev.SetPortValues(
                SVMBaudRate.ToString(),
                SVMDataBits,
                SVMStopbits,
                SVMParity,
                SVMHandShake,
                SVMPortName,
                BufferSize);
        }

        /// <summary>
        /// Returns true if it's closed but is now open. Returns false if it's already open and is now closed.
        /// </summary>
        /// <returns>a bool.</returns>
        public bool AutoconnectStream()
        {
            if (WritingStreamOpen)
            {
                CloseStream();
                return false;
            }
            else
            {
                OpenStream();
                return true;
            }
        }
        public void OpenStream()
        {
            if (!StreamHasPath)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                sfd.Title = "Where to save the stream to";
                if(sfd.ShowDialog() == DialogResult.OK)
                {
                    StreamPath = sfd.FileName;
                    StreamHasPath = true;
                }
            }

            //WriteStream = new FileStream(StreamPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            WriteStream = new FILESTREAM();
            WriteStream.FullPath = StreamPath;
            WriteStream.Open();
        }
        public void CloseStream()
        {
            WriteStream.FullOnClose();
        }
        public void WriteToStream(string data)
        {
            WriteStream.Write(data);
        }

        private void message(string data, string rxortx)
        {
            AddNewMessage(data, rxortx);
        }

        private void clrMessageBuffer()
        {
            MainText = "";
            CurrentMessageCount = 0;
        }
        private void clrBuffers()
        {
            serialDev.ClearBuffers();
        }

        private void connect()
        {
            string status = serialDev.AutoConnect();
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
            //Ports = new ObservableCollection<string>(SerialPort.GetPortNames());
            foreach (string v in SerialPort.GetPortNames())
                Ports.Add(v);
        }

        public void AddNewMessage(string data, string RXorTX)
        {
            //clear message buffer (aka the text) if there's too much text cus it will lag
            if (MainText != null && MainText.Length > MessageCount * /*number of characters*/ 50)
                clrMessageBuffer();

            //clear message buffer (aka the text) if the number of messages exceeds the limit.
            if (CurrentMessageCount > MessageCount)
                clrMessageBuffer();

            MainText += $"{RXorTX}> {data}" + '\n';
            CurrentMessageCount++;
        }

        /// <summary>
        /// if needed, adds custom messages to message buuufferrr
        /// </summary>
        /// <param name="message"></param>
        public void AddMessage(string message)
        {
            //clear message buffer (aka the text) if there's too much text cus it will lag
            if (MainText != null && MainText.Length > MessageCount * /*number of characters*/ 50)
                clrMessageBuffer();

            //clear message buffer (aka the text) if the number of messages exceeds the limit.
            if (CurrentMessageCount > MessageCount)
                clrMessageBuffer();

            MainText += $"{message}" + '\n';
            CurrentMessageCount++;
            if (WritingStreamOpen)
                WriteToStream(message);
        }

        private void startupMessage(string portname, string baudrate, string databits)
        {
            AddMessage($"Port:     {portname}{Environment.NewLine}");
            AddMessage($"Baudrate: {baudrate}{Environment.NewLine}");
            AddMessage($"DataBits: {databits}{Environment.NewLine}");
        }

        public void DisposeProcedure()
        {
            serialDev.DisposeProc();
        }

        #endregion
    }
}