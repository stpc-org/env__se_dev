// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MyAbstractControlMenuItem
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Text;
using VRage.Input;
using VRage.Utils;

namespace Sandbox.Game.Screens.Helpers
{
  public abstract class MyAbstractControlMenuItem
  {
    private static string CTRL = "ctrl";
    private static string SHIFT = "shift";
    private static string ALT = "alt";
    private static string PLUS = " + ";
    private static string COLON = ":";
    private static string FORMAT_LABEL = "{0}";
    private static string FORMAT_LABEL_HINT = "{0} ({1})";
    private static StringBuilder m_tmpBuilder = new StringBuilder();

    public abstract string Label { get; }

    public virtual string CurrentValue => (string) null;

    public MyStringId ControlCode { get; private set; }

    public string ControlName { get; private set; }

    public virtual bool Enabled => true;

    public string ControlLabel
    {
      get
      {
        if (string.IsNullOrEmpty(this.Label))
          return (string) null;
        MyAbstractControlMenuItem.m_tmpBuilder.Clear();
        if (string.IsNullOrEmpty(this.ControlName))
          MyAbstractControlMenuItem.m_tmpBuilder.AppendFormat(MyAbstractControlMenuItem.FORMAT_LABEL, (object) this.Label);
        else
          MyAbstractControlMenuItem.m_tmpBuilder.AppendFormat(MyAbstractControlMenuItem.FORMAT_LABEL_HINT, (object) this.Label, (object) this.ControlName);
        if (!string.IsNullOrEmpty(this.CurrentValue))
          MyAbstractControlMenuItem.m_tmpBuilder.Append(MyAbstractControlMenuItem.COLON);
        return MyAbstractControlMenuItem.m_tmpBuilder.ToString();
      }
    }

    public MyAbstractControlMenuItem(MyStringId controlCode, MySupportKeysEnum supportKeys = MySupportKeysEnum.NONE) => this.ControlName = this.ConstructCompleteControl(this.GetControlName(controlCode), supportKeys);

    public MyAbstractControlMenuItem(string controlName, MySupportKeysEnum supportKeys = MySupportKeysEnum.NONE) => this.ControlName = this.ConstructCompleteControl(controlName, supportKeys);

    public abstract void Activate();

    public virtual void Next()
    {
    }

    public virtual void Previous()
    {
    }

    public virtual void UpdateValue()
    {
    }

    private string GetControlName(MyStringId controlCode)
    {
      if (controlCode == MyStringId.NullOrEmpty)
        return (string) null;
      string str = (string) null;
      MyControl gameControl = MyInput.Static.GetGameControl(controlCode);
      if (gameControl != null)
      {
        MyMouseButtonsEnum mouseControl = gameControl.GetMouseControl();
        MyKeys keyboardControl = gameControl.GetKeyboardControl();
        if (mouseControl != MyMouseButtonsEnum.None)
          str = MyInput.Static.GetName(mouseControl);
        else if (keyboardControl != MyKeys.None)
          str = MyInput.Static.GetKeyName(keyboardControl);
      }
      return str;
    }

    private string ConstructCompleteControl(string controlName, MySupportKeysEnum supportKeys)
    {
      MyAbstractControlMenuItem.m_tmpBuilder.Clear();
      if (this.HasSupportKey(supportKeys, MySupportKeysEnum.CTRL))
        MyAbstractControlMenuItem.m_tmpBuilder.Append(MyAbstractControlMenuItem.CTRL).Append(MyAbstractControlMenuItem.PLUS);
      if (this.HasSupportKey(supportKeys, MySupportKeysEnum.SHIFT))
        MyAbstractControlMenuItem.m_tmpBuilder.Append(MyAbstractControlMenuItem.SHIFT).Append(MyAbstractControlMenuItem.PLUS);
      if (this.HasSupportKey(supportKeys, MySupportKeysEnum.ALT))
        MyAbstractControlMenuItem.m_tmpBuilder.Append(MyAbstractControlMenuItem.ALT).Append(MyAbstractControlMenuItem.PLUS);
      MyAbstractControlMenuItem.m_tmpBuilder.Append(controlName);
      return MyAbstractControlMenuItem.m_tmpBuilder.ToString();
    }

    private bool HasSupportKey(MySupportKeysEnum collection, MySupportKeysEnum key) => (collection & key) == key;
  }
}
