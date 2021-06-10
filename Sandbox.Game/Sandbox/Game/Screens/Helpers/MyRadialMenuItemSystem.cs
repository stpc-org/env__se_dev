// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MyRadialMenuItemSystem
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Screens.Helpers.RadialMenuActions;
using VRage.Game;

namespace Sandbox.Game.Screens.Helpers
{
  [MyRadialMenuItemDescriptor(typeof (MyObjectBuilder_RadialMenuItemSystem))]
  internal class MyRadialMenuItemSystem : MyRadialMenuItem
  {
    private MySystemAction m_systemAction;
    private IMyRadialMenuSystemAction m_action;

    public override MyRadialLabelText Label
    {
      get
      {
        if (this.m_action != null)
          return this.m_action.GetLabel(this.LabelShortcut, this.LabelName);
        return new MyRadialLabelText()
        {
          Name = this.LabelName,
          State = string.Empty,
          Shortcut = this.LabelShortcut
        };
      }
    }

    public override void Init(MyObjectBuilder_RadialMenuItem builder)
    {
      base.Init(builder);
      this.m_systemAction = (MySystemAction) ((MyObjectBuilder_RadialMenuItemSystem) builder).SystemAction;
      this.m_action = MyRadialMenuItemFactory.GetSystemMenuAction(this.m_systemAction);
    }

    public override void Activate(params object[] parameters)
    {
      if (!this.Enabled() || this.m_action == null)
        return;
      this.m_action.ExecuteAction();
    }

    public override string GetIcon()
    {
      if (this.Icons == null || this.Icons.Count <= 0 || this.m_action == null)
        return string.Empty;
      int iconIndex = this.m_action.GetIconIndex();
      return iconIndex < 0 || iconIndex >= this.Icons.Count ? string.Empty : this.Icons[iconIndex];
    }

    public override bool Enabled() => this.m_action != null && this.m_action.IsEnabled() && base.Enabled();
  }
}
