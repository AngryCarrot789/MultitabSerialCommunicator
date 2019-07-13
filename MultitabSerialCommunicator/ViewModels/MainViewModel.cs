using MultitabSerialCommunicator.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MultitabSerialCommunicator
{
    public class MainViewModel
    {
        public MainViewModel()
        {
            ShowInfoCommand = new DelegateCommand(showInfoWindow);
        }

        public ICommand ShowInfoCommand { get; set; }

        private void showInfoWindow()
        {
            new InfoWindow().Show();
        }
    }
}
