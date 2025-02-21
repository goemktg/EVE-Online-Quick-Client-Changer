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
        public List<EveClientData> EveClients;

        public MainWindow()
        {
            InitializeComponent();

            // Load settings from file
            // Disable reload button while loading to prevent bad things happen
            btnClientReload.IsEnabled = false;
            EveClients = SettingsHandler.LoadSettings();
            btnClientReload.IsEnabled = true;

            LoadEveClients();


            // TODO: Add form closing event: Compare Between settings.json and current settings
            // and ask user to save settings if there is any difference
            // Text will be: "저장되지 않은 변경사항이 있습니다. 저장하시겠습니까?"
        }

        private void LoadEveClients()
        {
            // Disable reload button while loading
            btnClientReload.IsEnabled = false;

            // Create keyMap for current eve online processes
            // Search for all EVE Online clients
            System.Diagnostics.Process[] eveProcesses = System.Diagnostics.Process.GetProcessesByName("exefile");
            Dictionary<string, int> eveClientsPairs = [];
            foreach (System.Diagnostics.Process process in eveProcesses)
            {
                // skip EVE clients in login screen
                if (process.MainWindowTitle == "EVE")
                    continue;

                eveClientsPairs.Add(process.MainWindowTitle, process.Id);
            }

            // Loop through ALL saved clients 
            foreach (EveClientData client in EveClients)
            {
                // Check if client is still running
                if (eveClientsPairs.ContainsKey(client.MainWindowTitle))
                {
                    // Update process ID
                    client.ProcessID = eveClientsPairs[client.MainWindowTitle];
                    // Update color
                    client.TextColor = "Black";
                    // Remove from Dictionary
                    eveClientsPairs.Remove(client.MainWindowTitle);
                }
                else
                {
                    // Gray out text if client is not running
                    client.TextColor = "Gray";
                }
            }

            // Loop through remaining clients
            foreach (KeyValuePair<string, int> client in eveClientsPairs)
            {
                // Add new client to list
                EveClients.Add(new EveClientData
                {
                    ProcessID = client.Value,
                    MainWindowTitle = client.Key,

                    // It is placeholder value
                    HotKeyName = "없음",
                    HotKeyID = -1,
                    TextColor = "Black"
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
                        LoadEveClients();
                        break;
                    case "btnClientSetKey":
                        // "클라이언트 키 지정" 버튼 클릭
                        // Set hotkey for selected client
                        break;
                    case "btnSave":
                        // "저장" 버튼 클릭
                        // Save settings as JSON settings
                        SettingsHandler.SaveSettings(EveClients);
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
        public required int ProcessID { get; set; }
        public required string MainWindowTitle { get; set; }
        public required string HotKeyName { get; set; }
        public required int HotKeyID { get; set; }
        public required string TextColor { get; set; }
    }
}