using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultitabSerialCommunicator
{
    public interface ISerialComms
    {
        void NewMessage(string data, string RX_or_TX);
    }
}
