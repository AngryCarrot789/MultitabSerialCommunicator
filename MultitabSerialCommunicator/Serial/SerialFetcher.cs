using System.Collections.Generic;
using System.IO.Ports;

namespace MultitabSerialCommunicator.Serial
{
    public class SerialFetcher
    {
        public static IEnumerable<string> GetSerialPorts()
        {
            foreach (string portName in SerialPort.GetPortNames())
            {
                yield return portName;
            }
        }
    }
}
