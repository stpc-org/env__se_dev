// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MyRadialMenuItemVoxelHandBrush
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using VRage;
using VRage.Game;
using VRage.Game.ObjectBuilders;

namespace Sandbox.Game.Screens.Helpers
{
  [MyRadialMenuItemDescriptor(typeof (MyObjectBuilder_RadialMenuItemVoxelHandBrush))]
  public class MyRadialMenuItemVoxelHandBrush : MyRadialMenuItem
  {
    private string m_brushSubtypeName;

    public override MyRadialLabelText Label
    {
      get
      {
        MyRadialLabelText label = new MyRadialLabelText();
        label.Name = this.LabelName;
        if (!MySession.Static.CreativeToolsEnabled(Sync.MyId) && !MySession.Static.CreativeMode)
          label.State = label.State + MyRadialMenuItemVoxelHandBrush.AppendingConjunctionState(label) + MyTexts.GetString(MySpaceTexts.RadialMenu_Label_CreativeOnly);
        else if (!MySession.Static.Settings.EnableVoxelHand)
          label.State = label.State + MyRadialMenuItemVoxelHandBrush.AppendingConjunctionState(label) + MyTexts.GetString(MySpaceTexts.RadialMenu_Label_DisabledWorld);
        label.Shortcut = this.LabelShortcut;
        return label;
      }
    }

    public override bool Enabled() => (MySession.Static.CreativeToolsEnabled(Sync.MyId) || MySession.Static.CreativeMode) && MySession.Static.Settings.EnableVoxelHand && base.Enabled();

    public override void Init(MyObjectBuilder_RadialMenuItem builder)
    {
      base.Init(builder);
      this.m_brushSubtypeName = ((MyObjectBuilder_RadialMenuItemVoxelHandBrush) builder).BrushSubtypeName;
    }

    public override void Activate(params object[] parameters)
    {
      int num = MySession.Static.CreativeMode ? 1 : (MySession.Static.IsUserAdmin(Sync.MyId) ? 1 : 0);
      if (num != 0)
        MySession.Static.GameFocusManager.Clear();
      if (num != 0)
        MySessionComponentVoxelHand.Static.EquipVoxelHand(this.m_brushSubtypeName);
      if (!MySessionComponentVoxelHand.Static.Enabled)
        return;
      MySession.Static.ControlledEntity?.SwitchToWeapon((MyToolbarItemWeapon) null);
    }

    protected static string AppendingConjunctionState(MyRadialLabelText label) => !string.IsNullOrEmpty(label.State) ? " - " : "";
  }
}
