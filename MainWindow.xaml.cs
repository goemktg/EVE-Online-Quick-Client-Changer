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
            // Disable reload button while loading
            btnClientReload.IsEnabled = false;

            // Clear listbox data
            EveClients.Clear();

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
            lbEveClients.Items.Refresh();

            // Enable reload button after loading
            btnClientReload.IsEnabled = true;
        }

        // Button click events
        private void ButtonClickEventsHandler(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                switch (button.Name)
                {
                    case "btnClientReload":
                        // "클라이언트 리로드" 버튼 클릭
                        // Reload client list
                        LoadListBoxData();
                        break;
                    case "btnClientSetKey":
                        // "클라이언트 키 지정" 버튼 클릭
                        // Set hotkey for selected client
                        break;
                    case "btnReset":
                        // "초기화" 버튼 클릭
                        // Nuke all settings ( need comfirm window )
                        break;
                    case "btnToggleHotkeys":
                        // "단축키 동작중" / "단축키 미동작중" 버튼 클릭
                        // Toggle hotkey usage
                        break;
                }
            }
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