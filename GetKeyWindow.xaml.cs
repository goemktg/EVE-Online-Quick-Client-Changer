using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EVE_Online_Quick_Client_Changer
{
    /// <summary>
    /// GetKeyWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class GetKeyWindow : Window
    {
        public string HotKeyName { get; private set; }
        public uint HotKeyVirtualKeyCode { get; private set; }

        public GetKeyWindow()
        {
            InitializeComponent();

            // Start with any value ( Not be used if form canceled )
            HotKeyName = string.Empty;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            HotKeyName = e.Key.ToString();
            HotKeyVirtualKeyCode = (uint)KeyInterop.VirtualKeyFromKey(e.Key);
            DialogResult = true;
            Close();
        }
    }
}
