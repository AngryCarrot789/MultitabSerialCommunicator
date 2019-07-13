using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultitabSerialCommunicator.ViewModels
{
    public class SerialDataCollections
    {
        public List<int> BaudRates { get; set; } = new List<int>
        {
            110, 300, 600, 1200, 2400, 4800, 9600, 19200, 38400, 57600, 74880, 115200, 128000, 256000
        };

        public List<string> Databits { get; set; } = new List<string>
        {
            "4", "5", "6", "7", "8"
        };

        public List<string> Stopbits { get; set; } = new List<string>
        {
            "None", "One", "OnePointFive", "Two"
        };

        public List<string> Parities { get; set; } = new List<string>
        {
            "Even", "Odd", "None", "Mark", "Space"
        };

        public List<string> Handshakes { get; set; } = new List<string>
        {
            "None", "RequestToSend", "RequestToSendXOnXOff", "XOnXOff"
        };
    }
}
