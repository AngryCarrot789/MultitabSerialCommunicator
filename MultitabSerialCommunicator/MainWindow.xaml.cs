using MahApps.Metro.Controls;
using MultitabSerialCommunicator.Views;
using System.Windows;
using System.Windows.Controls;

namespace MultitabSerialCommunicator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            addNew();
        }

        public int SelectedIndex { get { return main.SelectedIndex; } }

        private void addNew(object sender, RoutedEventArgs e)
        {
            addNew();
        }

        void addNew()
        {
            SerialView sview = new SerialView();
            sview.InitializeComponent();
            sview.DataContext = new SerialViewModel();

            TabItem ti = new TabItem();
            ContextMenu cm = new ContextMenu();
            MenuItem mi = new MenuItem();
            mi.Header = "Close";
            mi.Click += Mi_Click;
            mi.Uid = "0";
            cm.Items.Add(mi);

            ti.ContextMenu = cm;
            ti.Header = $"Serial {main.Items.Count}";
            ti.Content = sview;

            main.Items.Add(ti);
            main.SelectedIndex = main.Items.Count - 1;
        }

        private void Mi_Click(object sender, RoutedEventArgs e)
        {
            switch(int.Parse(((MenuItem)e.Source).Uid))
            {
                case 0:
                    {
                        try
                        {
                            (((
                                main.Items.                                   //All items
                                GetItemAt(main.SelectedIndex) as TabItem).    //Gets selected tabitem
                                Content as SerialView).                       //Casts content as SerialView
                                DataContext as SerialViewModel).              //Casts datacontext as SerialViewModel
                                DisposeProcedure();                           //Runs
                        }
                        catch { }
                        //try { (q.DataContext as SerialViewModel).DisposeProcedure(); } catch { }
                        main.Items.RemoveAt(main.SelectedIndex);
                    } break;
            }
        }
    }
}
