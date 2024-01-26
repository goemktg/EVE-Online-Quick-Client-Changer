using System.Windows.Forms;

namespace NumpadClientChanger
{
  public partial class GetKey : Form
  {
    MainForm mainForm;
    public GetKey(MainForm main)
    {
      InitializeComponent();
      mainForm = main;
    }

    private bool HotKeySeted = false;
    private int RegisteredHotKeyID;

    private void GetKey_KeyDown(object sender, KeyEventArgs e)
    {
      if (RegisteredCheck(e))
      {
        MainForm.UnregisterHotKey((int)mainForm.Handle, RegisteredHotKeyID); //이 폼에 ID가 0인 핫키 해제

        (mainForm.lb_EVEClientList.SelectedItem as listboxData).Key_value = 0;
        (mainForm.lb_EVEClientList.SelectedItem as listboxData).Key_data = null;
      }
        (mainForm.lb_EVEClientList.SelectedItem as listboxData).Key_value = e.KeyValue;
      (mainForm.lb_EVEClientList.SelectedItem as listboxData).Key_data = e.KeyData.ToString();
      MainForm.RegisterHotKey((int)mainForm.Handle, (mainForm.lb_EVEClientList.SelectedItem as listboxData).Hotkey_id, 0x0, e.KeyValue);
      mainForm.refrashlabel();
      HotKeySeted = true;
      Close();
    }

    private void GetKey_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (HotKeySeted)
        MainForm.RegisterHotKey((int)mainForm.Handle, (mainForm.lb_EVEClientList.SelectedItem as listboxData).Hotkey_id, 0x0, (mainForm.lb_EVEClientList.SelectedItem as listboxData).Key_value);
    }

    private bool RegisteredCheck(KeyEventArgs e)
    {
      // loop checks every listboxDataList items
      for (int index = 0; index <= mainForm.lb_EVEClientList.Items.Count - 1; index++)
      {
        foreach (listboxData listboxData in mainForm.lb_EVEClientList.Items)
        {
          if (e.KeyValue == listboxData.Key_value)
          {
            RegisteredHotKeyID = listboxData.Hotkey_id;
            return true;
          }
        }
      }
      return false;
    }
  }
}
