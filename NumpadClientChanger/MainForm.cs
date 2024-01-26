using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static System.Net.WebRequestMethods;
using System.Windows.Interop;

namespace NumpadClientChanger
{
  public partial class MainForm : Form
  {
    public List<listboxData> listboxDataList = new List<listboxData>();

    const int GWL_STYLE = (-16);
    const uint WS_MINIMIZE = 0x20000000;
    const int SW_RESTORE = 9;

    // 핫키등록
    [DllImport("user32.dll")]
    public static extern int RegisterHotKey(int hwnd, int id, int fsModifiers, int vk);

    // 핫키제거
    [DllImport("user32.dll")]
    public static extern int UnregisterHotKey(int hwnd, int id);

    // title명으로 찾기
    [DllImport("user32.dll")]
    private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    // 활성화 프로세스 찾기
    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    // 활성화 프로세스 가져오기
    [DllImport("user32.dll")]
    private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

    // 최소화시 활성화
    [DllImport("user32.dll")]
    private static extern bool SetForegroundWindow(IntPtr hWnd);

    // 젤위로
    [DllImport("user32.dll")]
    private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

    private bool is_onoffSwitchHotkey = false;

    public MainForm()
    {
      InitializeComponent();

      loadEveClientList();

      // 단축키 동작 on off 단축키 등록
      RegisterHotKey((int)Handle, 77777, 2 | 1, 67);
      //applyKeyMapData();

    }

    private void loadEveClientList()
    {
      Process[] processesByName = Process.GetProcessesByName("exefile");

      int hotkeyidIndex = 0;

      foreach (Process process in processesByName)
      {
        listboxDataList.Add(new listboxData()
        {
          ProcessID = process.Id,
          MainWindowTitle = process.MainWindowTitle,
          Hotkey_id = hotkeyidIndex
        });
        hotkeyidIndex++;
      }

      lb_EVEClientList.DataSource = listboxDataList;
      lb_EVEClientList.DisplayMember = "MainWindowTitle";
    }


    // return List of string List
    private List<List<string>> LoadKeyMapData()
    {
      string datastr = Properties.Settings.Default.keymapdata;
      string[] dataarray = datastr.Split(':');
      List<List<string>> keymapingdata = new List<List<string>>();
      foreach (string s in dataarray)
      {
        string front, middle, end;
        string[] strlist = s.Split('!');
        if (strlist.Length == 3)
        {
          front = strlist[0];
          middle = strlist[1];
          end = strlist[2];
        }
        else
        {
          front = "";
          middle = "";
          end = "";
        }
        keymapingdata.Add(new List<string>()
                {
                    front,middle,end
                });
      }

      return keymapingdata;
    }

    private void applyKeyMapData()
    {
      List<List<string>> KeyMapdata = LoadKeyMapData();

      // loop checks every listboxDataList items
      for (int index = 0; index <= KeyMapdata.Count - 1; index++)
      {
        foreach (listboxData listboxData in listboxDataList)
        {
          if (listboxData.MainWindowTitle == KeyMapdata[index][0])
          {
            listboxData.Key_value = int.Parse(KeyMapdata[index][1]);
            listboxData.Key_data = KeyMapdata[index][2];

            RegisterHotKey((int)Handle, listboxData.Hotkey_id, 0x0, listboxData.Key_value);
            break;
          }
        }
      }

      refrashlabel();
    }

    private void lb_EVEClientList_SelectedIndexChanged(object sender, EventArgs e)
    {
      refrashlabel();
    }

    private void btn_SelectClientKey_Click(object sender, EventArgs e)
    {
      UnregisterHotKey((int)Handle, lb_EVEClientList.SelectedIndex);

      GetKey getkey = new GetKey(this);
      getkey.ShowDialog();
    }

    public void refrashlabel()
    {
      if (lb_EVEClientList.SelectedItem == null) return;

      lb_ClientName.Text = (lb_EVEClientList.SelectedItem as listboxData).MainWindowTitle.ToString();
      lb_ClientKey.Text = "KVal : " + (lb_EVEClientList.SelectedItem as listboxData).Key_value.ToString();
      lb_keyid.Text = "HKID : " + (lb_EVEClientList.SelectedItem as listboxData).Hotkey_id;
      lb_KeyText.Text = "설정된 키 : " + (lb_EVEClientList.SelectedItem as listboxData).Key_data;
    }

    private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      UnregisterHotKey((int)Handle, 77777);
      unregisterAllHotKey();
    }
    protected override void WndProc(ref Message m) //윈도우프로시저 콜백함수
    {
      uint foregroundProcessID = 0;
      IntPtr foregroundHwnd = GetForegroundWindow();
      uint threadID = GetWindowThreadProcessId(foregroundHwnd, out foregroundProcessID);

      listboxData foregroundProcess = listboxDataList.Find(item => item.ProcessID == foregroundProcessID);
      if (m.Msg == (int)0x312) //핫키가 눌러지면 312 정수 메세지를 받게됨
      {
        if ((int)m.WParam == 77777)
        {
          onoffHotKey();
        }
      }

        if (foregroundProcess != null)
      {
        if (m.Msg == (int)0x312) //핫키가 눌러지면 312 정수 메세지를 받게됨
        {
          foreach (listboxData temp in listboxDataList)
          {
            if ((IntPtr)temp.Hotkey_id == m.WParam) ProcessCall(temp.MainWindowTitle);
          }
        }
      }

      base.WndProc(ref m);
    }
    // 프로세스 최상위로 호출
    private void ProcessCall(string processName)
    {
      //// 윈도우 타이틀명으로 핸들을 찾는다
      //IntPtr hWnd = FindWindow(null, processName);

      //if (!hWnd.Equals(IntPtr.Zero))
      //{
      //    // 윈도우가 최소화 되어 있다면 활성화 시킨다
      //    /*
      //    if (IsWindowFull(hWnd))
      //        ShowWindowAsync(hWnd, 3);//전체화면
      //    else ShowWindowAsync(hWnd, 1);//일반화면
      //    */
      //    // 윈도우에 포커스를 줘서 최상위로 만든다
      //    SetForegroundWindow(hWnd);
      //}
      IntPtr hWnd = FindWindow(null, processName);

      if (!hWnd.Equals(IntPtr.Zero))
      {
        ShowWindowAsync(hWnd, 1);
        SetForegroundWindow(hWnd);
      }
    }

    private void SaveKeyMapData(object sender, EventArgs e)
    {
      // save
      string keystring = "";
      foreach (listboxData listboxData in listboxDataList)
      {
        keystring += listboxData.MainWindowTitle + "!" + listboxData.Key_value + "!" + listboxData.Key_data + ":";
      }
      keystring = keystring.Substring(0, keystring.Length - 1);
      //MessageBox.Show(keystring);
      Properties.Settings.Default.keymapdata = keystring;
      Properties.Settings.Default.Save();
    }

    private void ResetListbox(object sender, EventArgs e)
    {
      // unreg. 
      unregisterAllHotKey();

      // delete all loaded data
      listboxDataList.Clear();
      lb_EVEClientList.DataSource = null;
      lb_EVEClientList.Items.Clear();

      // reset saved setting
      Properties.Settings.Default.Reset();
      Properties.Settings.Default.Save();

      // re-load eve online client
      loadEveClientList();
    }

    private void unregisterAllHotKey()
    {
      for (int i = 0; i < lb_EVEClientList.Items.Count; i++)
      {
        UnregisterHotKey((int)Handle, i); //이 폼에 ID가 0인 핫키 해제
      }
    }

    private void button3_Click(object sender, EventArgs e)
    {
      if (lb_EVEClientList.Items.Count > 0)
      {
        lb_EVEClientList.DataSource = null;
        listboxDataList = null;
        listboxDataList = new List<listboxData>();
      }
        loadEveClientList();
    }
    private void onoffHotKey()
    {
      is_onoffSwitchHotkey = !is_onoffSwitchHotkey;
      if (is_onoffSwitchHotkey)
      {
        button4.Text = "단축키 동작중";
        button4.ForeColor = System.Drawing.Color.White;
        button4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
        applyKeyMapData();
      }
      else
      {
        button4.Text = "단축키 미동작중";
        button4.ForeColor = Button.DefaultForeColor;
        button4.BackColor = Button.DefaultBackColor;
        unregisterAllHotKey();
        refrashlabel();
      }
    }

    private void button4_Click(object sender, EventArgs e)
    {
      onoffHotKey();
    }
  }
}
