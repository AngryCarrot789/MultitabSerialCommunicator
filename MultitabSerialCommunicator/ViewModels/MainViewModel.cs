using MultitabSerialCommunicator.Resources;
using MultitabSerialCommunicator.Views;
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
        public ICommand OpenConfigWindowCommand { get; set; }
        public MainViewModel() {
            NewTabCommand = new DelegateCommand(NewTab);
            OpenConfigWindowCommand = new DelegateCommand(OpnConfigWnd);
        }

        public Configuration config = new Configuration();
        private void OpnConfigWnd() {
            config.Show();
            config.Callback = ConfigCallback;
        }

        private void ConfigCallback(ConfigTypes ct, int val) {
            if (ct == ConfigTypes.MaxMessagesReceivable) {
                (((Tabs[SelectedIndex] as TabItem).Content as SerialView).DataContext as SerialViewModel).MessageCount = val;
            }
        }

        public void NewTab() {
            ContextMenu cm = new ContextMenu();
            MenuItem mi = new MenuItem {
                Header = "Close",
                Uid = "0"
            };
            mi.Click += Mi_Click;
            cm.Items.Add(mi);
            TabItem ti = new TabItem() {
                ContextMenu = cm,
                Header = $"Serial {Tabs.Count}",
                Content = new SerialView()
            };

            Tabs.Add(ti);
            SelectedIndex = Tabs.Count - 1;
        }

        private void Mi_Click(object sender, RoutedEventArgs e) {
            switch (int.Parse(((MenuItem)e.Source).Uid)) {
                case 0: {
                    try {
                        (((
                            Tabs                              //All items
                            [SelectedIndex] as TabItem).      //Gets selected tabitem
                            Content as SerialView).           //Casts content as SerialView
                            DataContext as SerialViewModel).  //Casts datacontext as SerialViewModel
                            DisposeProcedure();               //Runs
                    }
                    catch { }
                    //try { (q.DataContext as SerialViewModel).DisposeProcedure(); } catch { }
                    Tabs.RemoveAt(SelectedIndex);
                }
                break;
            }
        }
    }
}