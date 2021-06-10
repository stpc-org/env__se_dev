// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Blocks.MyDecoy
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Game.Entities.Cube;
using Sandbox.ModAPI;
using System;
using VRage.Game;
using VRage.Network;

namespace Sandbox.Game.Entities.Blocks
{
  [MyCubeBlockType(typeof (MyObjectBuilder_Decoy))]
  [MyTerminalInterface(new Type[] {typeof (Sandbox.ModAPI.IMyDecoy), typeof (Sandbox.ModAPI.Ingame.IMyDecoy)})]
  public class MyDecoy : MyFunctionalBlock, Sandbox.ModAPI.IMyDecoy, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyDecoy
  {
    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid) => base.Init(objectBuilder, cubeGrid);

    public override void OnAddedToScene(object source)
    {
      base.OnAddedToScene(source);
      this.CubeGrid.RegisterDecoy(this);
    }

    public override void OnRemovedFromScene(object source)
    {
      base.OnRemovedFromScene(source);
      this.CubeGrid.UnregisterDecoy(this);
    }

    public float GetSafetyRodRadius()
    {
      if (!(this.BlockDefinition is MyDecoyDefinition))
        return 0.0f;
      return this.CubeGrid.GridSizeEnum != MyCubeSize.Large ? ((MyDecoyDefinition) this.BlockDefinition).LightningRodRadiusSmall : ((MyDecoyDefinition) this.BlockDefinition).LightningRodRadiusLarge;
    }

    private class Sandbox_Game_Entities_Blocks_MyDecoy\u003C\u003EActor : IActivator, IActivator<MyDecoy>
    {
      object IActivator.CreateInstance() => (object) new MyDecoy();

      MyDecoy IActivator<MyDecoy>.CreateInstance() => new MyDecoy();
    }
  }
}
