// Decompiled with JetBrains decompiler
// Type: VRage.ModAPI.MyTerminalControlListBoxItem
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using VRage.Utils;

namespace VRage.ModAPI
{
  public class MyTerminalControlListBoxItem
  {
    public MyStringId Text { get; set; }

    public MyStringId Tooltip { get; set; }

    public object UserData { get; set; }

    public MyTerminalControlListBoxItem(MyStringId text, MyStringId tooltip, object userData)
    {
      this.Text = text;
      this.Tooltip = tooltip;
      this.UserData = userData;
    }
  }
}
