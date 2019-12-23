using MultitabSerialCommunicator.Resources;
using MultitabSerialCommunicator.Views;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MultitabSerialCommunicator
{
    public class MainViewModel : BaseViewModel
    {
        private ObservableCollection<TabItem> tabs = new ObservableCollection<TabItem>();
        public ObservableCollection<TabItem> Tabs { get => tabs; set { tabs = value; RaisePropertyChanged(); } }
        private int selectedIndex;
        public int SelectedIndex { get => selectedIndex; set { selectedIndex = value; RaisePropertyChanged(); } }
        public ICommand NewTabCommand { get; set; }
        public ICommand CloseTabCommand { get; set; }
        public ICommand OpenConfigWindowCommand { get; set; }

        public ICommand WriteToFileCommand { get; set; }
        public ICommand ReadFromFileCommand { get; set; }
        public ICommand BeginWriteStreamCommand { get; set; }
        private string bginStrTxt;
        public string WriteStreamText { get => bginStrTxt; set { bginStrTxt = value; RaisePropertyChanged(); } }
        public RenameWindow RenameWin { get; set; }
        public MainViewModel()
        {
            WriteStreamText = "Begin Writing Stream (DOESNT WORK (yet) JUST IGNORE!)";
            NewTabCommand = new DelegateCommand(NewTab);
            OpenConfigWindowCommand = new DelegateCommand(OpnConfigWnd);
            CloseTabCommand = new DelegateCommand(CloseSelectedTab);
            WriteToFileCommand = new DelegateCommand(writeFile);
            ReadFromFileCommand = new DelegateCommand(readFile);
            BeginWriteStreamCommand = new DelegateCommand(beginWrite);
            RenameWin = new RenameWindow();
        }
        private void writeFile()
        {
        }
        private void readFile() { }
        private void beginWrite()
        {
            //icantcodeverywellrip:(
            //if (ItemSelected && HasItems)
            //{
            //    var ee = (Tabs[SelectedIndex].Content as SerialView);
            //    if (ee != null)
            //    {
            //        if ((ee.DataContext as SerialViewModel).AutoconnectStream())
            //            WriteStreamText = "Close Writing Stream";
            //        else
            //        {
            //            WriteStreamText = "Begin Writing Stream";
            //        }
            //    }
            //}
        }
        public Configuration config = new Configuration();
        private void OpnConfigWnd()
        {
            config.Show();
            config.Callback = ConfigCallback;
        }

        public void ShowRenameWindow() { try { RenameWin.Tab = Tabs[SelectedIndex]; } catch { } RenameWin.Display(); }

        private void ConfigCallback(ConfigTypes ct, int val)
        {
            if (ct == ConfigTypes.MaxMessagesReceivable)
            {
                try
                {
                    (((Tabs[SelectedIndex] as TabItem).Content as SerialView).DataContext as SerialViewModel).MessageCount = val;
                }
                catch { }
            }
        }

        public void NewTab()
        {
            ContextMenu cm = new ContextMenu();
            MenuItem mi = new MenuItem
            {
                Header = "Close",
                Uid = "0"
            };
            MenuItem mi2 = new MenuItem
            {
                Header = "Rename",
                Uid = "1"
            };
            mi.Click += Mi_Click;
            mi2.Click += Mi_Click;
            cm.Items.Add(mi);
            cm.Items.Add(mi2);
            TabItem ti = new TabItem()
            {
                ContextMenu = cm,
                Header = $"Serial {Tabs.Count}",
                Content = new SerialView()
            };

            Tabs.Add(ti);
            SelectedIndex = Tabs.Count - 1;
        }

        private void Mi_Click(object sender, RoutedEventArgs e)
        {
            switch (int.Parse(((MenuItem)sender).Uid))
            {
                case 0: { CloseSelectedTab(); } break;
                case 1: { RenameTab(); } break;
            }
        }

        private void RenameTab()
        {
            ShowRenameWindow();
        }

        private void CloseSelectedTab()
        {
            if (ItemSelected)
            {
                try
                {
                    (((
                        Tabs                              //All items
                        [SelectedIndex] as TabItem).      //Gets selected tabitem
                        Content as SerialView).           //Casts content as SerialView
                        DataContext as SerialViewModel).  //Casts datacontext as SerialViewModel
                        DisposeProcedure();               //Runs
                    Tabs.RemoveAt(SelectedIndex);
                }
                catch { }
                //try { (q.DataContext as SerialViewModel).DisposeProcedure(); } catch { }
            }
        }

        public void KeyDown(Key key, bool[] extraKeys)
        {
            if (key == Key.Delete || key == Key.Escape)
                CloseSelectedTab();
            if (extraKeys[Convert.ToInt32(Key.LeftCtrl)])
            {
                if (key == Key.N)
                    NewTab();
                if (key == Key.Enter)
                    OpnConfigWnd();
                if (key == Key.R)
                    ShowRenameWindow();
            }
        }

        public bool ItemSelected => SelectedIndex >= 0;
        public bool HasItems => Tabs.Count > 0;
    }
}