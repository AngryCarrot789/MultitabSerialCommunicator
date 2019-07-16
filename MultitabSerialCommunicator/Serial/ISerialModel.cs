using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultitabSerialCommunicator
{
    public interface ISerialModel
    {
        Action<string, string> OnMessage { get; set; }
    }
}
