// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MyToolbarItemVoxelHand
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using VRage.Game;
using VRage.Game.Entity;

namespace Sandbox.Game.Screens.Helpers
{
  [MyToolbarItemDescriptor(typeof (MyObjectBuilder_ToolbarItemVoxelHand))]
  public class MyToolbarItemVoxelHand : MyToolbarItemDefinition
  {
    public override bool Init(MyObjectBuilder_ToolbarItem objBuilder)
    {
      base.Init(objBuilder);
      this.WantsToBeSelected = false;
      this.ActivateOnClick = false;
      return true;
    }

    public override bool Activate()
    {
      if (this.Definition == null || !MySessionComponentVoxelHand.Static.TrySetBrush(this.Definition.Id.SubtypeName))
        return false;
      bool flag = MySession.Static.CreativeMode || MySession.Static.IsUserAdmin(Sync.MyId);
      if (flag)
        MySession.Static.GameFocusManager.Clear();
      MySessionComponentVoxelHand.Static.Enabled = flag;
      if (!MySessionComponentVoxelHand.Static.Enabled)
        return false;
      MySessionComponentVoxelHand.Static.CurrentDefinition = this.Definition as MyVoxelHandDefinition;
      MySession.Static.ControlledEntity?.SwitchToWeapon((MyToolbarItemWeapon) null);
      return true;
    }

    public override bool AllowedInToolbarType(MyToolbarType type) => type == MyToolbarType.Character || type == MyToolbarType.Spectator;

    public override MyToolbarItem.ChangeInfo Update(MyEntity owner, long playerID = 0)
    {
      if (MySessionComponentVoxelHand.Static == null)
        return MyToolbarItem.ChangeInfo.None;
      MyVoxelHandDefinition voxelHandDefinition = MySessionComponentVoxelHand.Static.Enabled ? MySessionComponentVoxelHand.Static.CurrentDefinition : (MyVoxelHandDefinition) null;
      this.WantsToBeSelected = MySessionComponentVoxelHand.Static.Enabled && voxelHandDefinition != null && voxelHandDefinition.Id.SubtypeId == (this.Definition as MyVoxelHandDefinition).Id.SubtypeId;
      return MyToolbarItem.ChangeInfo.None;
    }
  }
}
