using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultitabSerialCommunicator.Views
{
    public interface ISerialView
    {
        Action<bool> SetAutoscroll { get; set; }
    }
}
