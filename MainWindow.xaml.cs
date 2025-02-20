using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EVE_Online_Quick_Client_Changer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<EveClientData> EveClients = [];

        public MainWindow()
        {
            InitializeComponent();

            LoadListBoxData();
        }

        private void LoadListBoxData()
        {
            // Search for all EVE Online clients
            System.Diagnostics.Process[] eveProcesses = System.Diagnostics.Process.GetProcessesByName("exefile");

            string hotKeyNamePlaceholder = "없음";
            int hotKeyIDPlaceholder = -1;

            foreach (System.Diagnostics.Process process in eveProcesses)
            {
                // skip EVE clients in login screen
                if (process.MainWindowTitle == "EVE")
                    continue;

                EveClients.Add(new EveClientData()
                {
                    ProcessID = process.Id,
                    MainWindowTitle = process.MainWindowTitle,
                    HotKeyName = hotKeyNamePlaceholder,
                    HotKeyID = hotKeyIDPlaceholder
                });
            }

            // Load data to listbox
            lbEveClients.ItemsSource = EveClients;
        }
    }

    public class EveClientData
    {
        public int ProcessID { get; set; }
        public required string MainWindowTitle { get; set; }
        public required string HotKeyName { get; set; }
        public int HotKeyID { get; set; }
    }
}