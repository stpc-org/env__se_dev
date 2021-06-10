// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MyRadialMenuItem
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Utils;

namespace Sandbox.Game.Screens.Helpers
{
  public abstract class MyRadialMenuItem
  {
    public List<string> Icons;
    public bool CloseMenu;

    public virtual MyRadialLabelText Label => new MyRadialLabelText()
    {
      Name = this.LabelName,
      State = string.Empty,
      Shortcut = this.LabelShortcut
    };

    public string LabelName { get; protected set; }

    public string LabelShortcut { get; protected set; }

    public virtual void Init(MyObjectBuilder_RadialMenuItem builder)
    {
      this.Icons = new List<string>();
      if (builder.Icons != null)
      {
        foreach (string icon in builder.Icons)
          this.Icons.Add(icon);
      }
      this.LabelName = MyTexts.GetString(builder.LabelName);
      MyStringId labelShortcut = builder.LabelShortcut;
      this.LabelShortcut = MyTexts.GetString(builder.LabelShortcut);
      this.CloseMenu = builder.CloseMenu;
    }

    public virtual bool Enabled() => true;

    public virtual bool CanBeActivated => this.Enabled();

    public abstract void Activate(params object[] parameters);

    public virtual string GetIcon() => this.Icons == null || this.Icons.Count <= 0 ? string.Empty : this.Icons[0];

    public virtual bool IsValid => true;
  }
}
