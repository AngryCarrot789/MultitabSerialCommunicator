using MultitabSerialCommunicator.ViewModels;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Text;
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
        private SerialDev serialDev = new SerialDev();
        public SerialDataCollections SerialDataCollections { get; set; } = new SerialDataCollections();
        #endregion

        #region public fields
        public int    SVMBaudRate                 { get { return baudRate;  } set { baudRate = value;  RaisePropertyChanged(); } }
        public string SVMPortName                 { get { return portName;  } set { portName = value; serialDev.SetPortName(value);  RaisePropertyChanged(); } }
        public string SVMDataBits                 { get { return dataBits;  } set { dataBits = value;  RaisePropertyChanged(); } }
        public string SVMStopbits                 { get { return stopBits;  } set { stopBits = value;  RaisePropertyChanged(); } }
        public string SVMParity                   { get { return parity;    } set { parity = value;    RaisePropertyChanged(); } }
        public string SVMHandShake                { get { return handShake; } set { handShake = value; RaisePropertyChanged(); } }
        public ObservableCollection<string> Ports { get { return ports;     } set { ports = value;     RaisePropertyChanged(); } }
        public string MainText                    { get { return mainText; }  set { mainText = value;  RaisePropertyChanged(); } }
        public string SendText                    { get { return sendText; }  set { sendText = value;  RaisePropertyChanged(); } }
        public string ButtonText                  { get { return btnText; }   set { btnText = value;   RaisePropertyChanged(); } }
        public ICommand ConnectToPort     { get; set; }
        public ICommand SendSerialMessage { get; set; }
        public ICommand RefreshCOMsList   { get; set; }
        #endregion

        #region Constructor

        public SerialViewModel()
        {
            ConnectToPort     = new DelegateCommand(connect);
            SendSerialMessage = new DelegateCommand(sendMessage);
            RefreshCOMsList   = new DelegateCommand(refreshList);
            SVMBaudRate       = 115200;
            SVMDataBits       = "8";
            SVMStopbits       = "One";
            SVMParity         = "None";
            SVMHandShake      = "None";
            ButtonText        = "Connect";
            serialDev.SetPortValues(SVMBaudRate.ToString(), SVMDataBits, SVMStopbits, SVMParity, SVMHandShake, Encoding.ASCII, "", "\n");
            refreshList();
        }

        #endregion

        #region Methods

        private void connect()
        {
            ButtonText = serialDev.AutoConnectToArduino();
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

        public void AddNewMessage(string RXorTX, string data)
        {
            MainText += $"{RXorTX}> {data}" + '\n';
        }

        #endregion

        #region DI
        public static SerialViewModel serialViewModel = new SerialViewModel();
        #endregion
    }
}
