using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NumpadClientChanger
{
    static class Program
    {
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }

    public class listboxData
    {
        public int ProcessID { get; set; }

        public string MainWindowTitle { get; set; }

        public int Key_value { get; set; }

        public int Hotkey_id { get; set; }

        public string Key_data { get; set; }
    }
}
