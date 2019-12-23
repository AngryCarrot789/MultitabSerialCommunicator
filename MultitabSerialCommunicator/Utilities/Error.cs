using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MultitabSerialCommunicator.Utilities
{
    public class Error
    {
        public static void Show(string title, string text) { MessageBox.Show(text, title); }
        public static void Show(string text) { MessageBox.Show(text); }
    }
}
