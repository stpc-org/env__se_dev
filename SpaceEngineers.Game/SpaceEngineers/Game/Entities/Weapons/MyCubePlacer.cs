// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.Entities.Weapons.MyCubePlacer
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using VRage.Game;
using VRage.Game.Entity;
using VRage.ObjectBuilders;
using VRageMath;

namespace SpaceEngineers.Game.Entities.Weapons
{
  [MyEntityType(typeof (MyObjectBuilder_CubePlacer), true)]
  public class MyCubePlacer : MyBlockPlacerBase
  {
    private static MyDefinitionId m_handItemDefId = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_CubePlacer));

    protected override MyBlockBuilderBase BlockBuilder => (MyBlockBuilderBase) MyCubeBuilder.Static;

    public override bool IsSkinnable => false;

    public MyCubePlacer()
      : base(MyDefinitionManager.Static.TryGetHandItemDefinition(ref MyCubePlacer.m_handItemDefId))
    {
    }

    public override void Shoot(
      MyShootActionEnum action,
      Vector3 direction,
      Vector3D? overrideWeaponPos,
      string gunAction)
    {
      if ((!MySession.Static.CreativeToolsEnabled(Sync.MyId) || !MySession.Static.HasCreativeRights ? (MySession.Static.CreativeMode ? 1 : 0) : 1) != 0)
        return;
      base.Shoot(action, direction, overrideWeaponPos, gunAction);
      if (action != MyShootActionEnum.PrimaryAction || this.m_firstShot || (MySandboxGame.TotalGamePlayTimeInMilliseconds - this.m_lastKeyPress < 500 || this.GetTargetBlock() == null) || !this.Owner.CanSwitchToWeapon(new MyDefinitionId?(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_Welder)))))
        return;
      this.Owner.SetupAutoswitch(new MyDefinitionId?(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_Welder))), new MyDefinitionId?(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_CubePlacer))));
    }
  }
}
