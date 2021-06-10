// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MyToolbarItemCreateGrid
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using VRage.Game;
using VRage.Game.Entity;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace Sandbox.Game.Screens.Helpers
{
  [MyToolbarItemDescriptor(typeof (MyObjectBuilder_ToolbarItemCreateGrid))]
  public class MyToolbarItemCreateGrid : MyToolbarItemDefinition
  {
    private static MyStringHash CreateSmallShip = MyStringHash.GetOrCompute(nameof (CreateSmallShip));
    private static MyStringHash CreateLargeShip = MyStringHash.GetOrCompute(nameof (CreateLargeShip));
    private static MyStringHash CreateStation = MyStringHash.GetOrCompute(nameof (CreateStation));

    public override bool Init(MyObjectBuilder_ToolbarItem objBuilder)
    {
      base.Init(objBuilder);
      this.WantsToBeSelected = false;
      this.WantsToBeActivated = true;
      this.ActivateOnClick = true;
      return true;
    }

    private void CreateGrid(MyCubeSize cubeSize, bool isStatic)
    {
      if (MySandboxGame.IsPaused)
        return;
      MySessionComponentVoxelHand.Static.Enabled = false;
      MyCubeBuilder.Static.StartStaticGridPlacement(cubeSize, isStatic);
      MyCharacter localCharacter = MySession.Static.LocalCharacter;
      if (localCharacter == null)
        return;
      MyDefinitionId weaponDefinition = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_CubePlacer));
      localCharacter.SwitchToWeapon(weaponDefinition);
    }

    public override bool Activate()
    {
      if (this.Definition.Id.SubtypeId == MyToolbarItemCreateGrid.CreateStation)
        this.CreateGrid(MyCubeSize.Large, true);
      return false;
    }

    public override bool AllowedInToolbarType(MyToolbarType type) => type == MyToolbarType.Character || type == MyToolbarType.Spectator;

    public override MyToolbarItem.ChangeInfo Update(MyEntity owner, long playerID = 0) => MyToolbarItem.ChangeInfo.None;
  }
}
