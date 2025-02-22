using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
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
        private List<EveClientData> EveClients;
        //                 hotKeyID, (hotKeyVirtualKeyCode, ClientHandle)
        private Dictionary<int, HotKeyData> RegisteredHotkeys = [];
        private IntPtr hwnd;

        private readonly int hotKeyInitialID = 9000;

        // Using Windows API to register hotkeys
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            hwnd = new WindowInteropHelper(this).Handle;

            // Load settings from file
            // Disable reload button while loading to prevent bad things happen
            btnClientReload.IsEnabled = false;
            EveClients = SettingsHandler.LoadSettings();
            btnClientReload.IsEnabled = true;

            LoadEveClients();

            // Disable SetClientKey button if no client is selected
            if (lbEveClients.SelectedIndex == -1)
            {
                btnSetClientKey.IsEnabled = false;
            }

            // Register hotkey hook
            HwndSource source = HwndSource.FromHwnd(hwnd);
            source.AddHook(HwndHook);
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

                    // check if hotkey is set
                    if (client.HotKeyVirtualKeyCode != 0)
                    {
                        try { RegisterHotKeyWrapper(client.HotKeyVirtualKeyCode, client.ProcessID); }
                        catch (Exception)
                        {
                            MessageBox.Show("단축키 등록에 실패했습니다. 다른 키를 선택해주세요.");
                            client.HotKeyName = "없음";
                            client.HotKeyVirtualKeyCode = 0;
                        }
                    }
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
                    HotKeyVirtualKeyCode = 0,
                    TextColor = "Black"
                });
            }

            // Load data to listbox
            lbEveClients.ItemsSource = EveClients;
            lbEveClients.Items.Refresh();

            // Enable reload button after loading
            btnClientReload.IsEnabled = true;
        }

        private void RegisterHotKeyWrapper(uint hotKeyVirtualKeyCode, int processID)
        {
            // Register hotkey if not already registered
            if (!IsThisKeyCodeAlreadyRegistered(hotKeyVirtualKeyCode))
            {
                // Register hotkey
                // TODO: Can add modifier key support here
                //       Like Ctrl + C, Alt + C, etc
                int hotKeyIndex = hotKeyInitialID + RegisteredHotkeys.Count;

                if (!RegisterHotKey(hwnd, hotKeyIndex, 0, hotKeyVirtualKeyCode))
                {
                    throw new Exception();
                }
                else
                {
                    // Add to registered hotkeys
                    RegisteredHotkeys.Add(hotKeyIndex, new HotKeyData { HotKeyVirtualKeyCode = hotKeyVirtualKeyCode, ProcessID = processID });
                }
            }
        }

        private bool IsThisKeyCodeAlreadyRegistered(uint hotKeyVirtualKeyCode)
        {
            foreach (var hotkey in RegisteredHotkeys)
            {
                if (hotkey.Value.HotKeyVirtualKeyCode == hotKeyVirtualKeyCode)
                    return true;
            }

            return false;
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
                    case "btnSetClientKey":
                        // "클라이언트 키 지정" 버튼 클릭
                        // Set hotkey for selected client

                        // SelectedIndex Will return -1 if no item is selected
                        if (lbEveClients.SelectedIndex == -1)
                        {
                            MessageBox.Show("단축키를 지정할 클라이언트가 선택되지 않았습니다.");
                            return;
                        }

                        GetKeyWindow getKeyWindow = new();
                        bool? result = getKeyWindow.ShowDialog();

                        if (result == true)
                        {
                            string hotKeyName = getKeyWindow.HotKeyName;
                            uint hotKeyVirtualKeyCode = getKeyWindow.HotKeyVirtualKeyCode;

                            // Update selected client hotkey
                            EveClients[lbEveClients.SelectedIndex].HotKeyName = hotKeyName;
                            EveClients[lbEveClients.SelectedIndex].HotKeyVirtualKeyCode = hotKeyVirtualKeyCode;

                            // Reload data to listbox
                            lbEveClients.ItemsSource = EveClients;
                            lbEveClients.Items.Refresh();

                            // Refresh selected client info
                            // TODO: Better way to do this?
                            lbEveClients_SelectionChanged(lbEveClients, null);
                        }
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

        private void lbEveClients_SelectionChanged(object sender, SelectionChangedEventArgs? e)
        {
            if (lbEveClients.SelectedIndex == -1)
            {
                btnSetClientKey.IsEnabled = false;
            }
            else
            {
                btnSetClientKey.IsEnabled = true;
            }

            // Update selected client info
            // Split by space and get 3rd element
            // EVE - Goem Funila -> Goem Funaila
            lblSelectedClientName.Content = "클라명: " + ((EveClientData)lbEveClients.SelectedItem).MainWindowTitle.Split(" - ")[1];
            lblSelectedClientHotKey.Content = "설정된 키: " + ((EveClientData)lbEveClients.SelectedItem).HotKeyName;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // TODO: Add form closing event: Compare Between settings.json and current settings
            // and ask user to save settings if there is any difference
            // Text will be: "저장되지 않은 변경사항이 있습니다. 저장하시겠습니까?"

            foreach (var hotkey in RegisteredHotkeys)
            {
                UnregisterHotKey(hwnd, hotkey.Key);
            }
        }

        private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_HOTKEY = 0x312;
            if (msg == WM_HOTKEY)
            {
                int hotkeyId = wParam.ToInt32();
                if (RegisteredHotkeys.ContainsKey(hotkeyId))
                {
                    // Handle the hotkey press
                    MessageBox.Show($"Hotkey {hotkeyId} pressed!");
                    handled = true;
                }
                else
                {
                    MessageBox.Show($"Hotkey {hotkeyId} not found! Something wrong...");
                }
            }
            return IntPtr.Zero;
        }
    }

    struct HotKeyData
    {
        public uint HotKeyVirtualKeyCode;
        public int ProcessID;
    }

    public class EveClientData
    {
        public required int ProcessID { get; set; }
        public required string MainWindowTitle { get; set; }
        public required string HotKeyName { get; set; }
        public required uint HotKeyVirtualKeyCode { get; set; }
        public required string TextColor { get; set; }
    }
}