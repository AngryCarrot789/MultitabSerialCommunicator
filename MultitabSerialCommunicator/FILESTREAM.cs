using MultitabSerialCommunicator.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MultitabSerialCommunicator
{
    public class FILESTREAM
    {
        public bool IsOpen { get; set; }
        public string FullPath { get; set; }

        private FileStream fS;
        public void Open()
        {
            if (!string.IsNullOrEmpty(FullPath))
                fS = new FileStream(FullPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            else
            {
                Error.Show("Path not set");
            }
        }

        public void Write(string data)
        {
            try
            {
                Open();
                byte[] bites = Encoding.ASCII.GetBytes(data);
                fS.Write(bites, 0, bites.Length);
                fS.Close();
            }
            catch(Exception ee) { Error.Show(ee.Message); }
        }

        /// <summary>
        /// Dispose everything.
        /// </summary>
        public void FullOnClose()
        {
            fS.Close();
            fS.Dispose();
        }
    }
}
